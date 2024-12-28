using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Windows.Markup;
using System.Linq;
using System.CodeDom;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using System.Configuration;
using System.Net.NetworkInformation;
using Persiscope.Lib;
using Persiscope.Common;
using System.Runtime.CompilerServices;
using Persiscope.Common;

namespace Persiscope.Hardware.Pico.Picov1
{
    public class Rp2DaqMchInterface : IDaqInterface
    {
        //public static AdcChannelInfo[] Channels;//= InitChannels();

        //static readonly int[] AdcPins = new int[] { 28, 27, 29 };//adc pin for each channel
        //static readonly int[] SwPins = new int[] { 19, 21, 22 };//switch pin for each channel
        //static readonly int[] AcDcPins = new int[] { 20, -1, -1 };//switch pin for each channel

        public bool StopFlag { get; set; }


        public int GetMaxChannelCount()
        {
            return 5;
        }


        public void Init()
        {
        }

        public static int GetChannelMask(params Rp2040AdcChannels[] gpios)
        {
            if (gpios.Contains(Rp2040AdcChannels.None))
                throw new Exception();

            var buf = gpios.Sum(i => (int)i);

            return buf;
        }

        //private AdcChannelInfo Channel;
        public Rp2daqMchCalibrationData CalibrationData;
        public Rp2daqMchUserSettings UserSettings;

        [Obsolete("use UserSettings instead")]
        public Rp2040AdcChannels[] ActiveChannels;



        static readonly byte Sop = 0x03;//start of package for rp2daq

        SnifferSerial Port;

        public bool IsConnected = false;


        public Rp2DaqMchInterface(Rp2daqMchUserSettings setts, Rp2daqMchCalibrationData calib)
        {
            //AdcResolutionBits = adcResolutionBits;
            //AdcSampleRate = adcSampleRate;
            //PortName = portName;

            UserSettings = setts ;
            CalibrationData = calib ;
        }
        

        /*
        public Rp2DaqInterface(string portName, long adcSampleRate)
        {
            //AdcResolutionBits = adcResolutionBits;
            AdcSampleRate = adcSampleRate;
            PortName = portName;
        }*/


        public double AdcMaxVoltage { get { return 3.3; } }


        public int AdcResolutionBits
        {
            get { return resolutionBits; }
            set
            {
                resolutionBits = value;
            }
        }

        private int resolutionBits = 12;


        public long AdcSampleRate { get; set; }




        //public int SampleRate ;
        public string PortName;

        public long TotalReads;

        //public byte BitWidth = 12;


        public static ushort blockSize = 1_200;
        public static ushort blocksToSend = 10;
        public static bool infiniteBlocks = true;
        //public int ChannelMask = 4;


        public bool Stopped = false;

        public DataRepository TargetRepository { get; set; }


        //private Queue<byte[]> Readed = new Queue<byte[]>();//those are filled with data
        //private Queue<byte[]> Emptied = new Queue<byte[]>();//those that content are used and ready to be reused
        //private object RLock = new object();//for Readed
        //private object ELock = new object();//for ELock


        public void DisConnect(bool log = false)
        {
            if (Port != null)
                Port.Close();

            IsConnected = false;
        }

        public void Connect(bool log = false)
        {
            var sport = new SnifferSerial(UserSettings.ComPortName, 268435456);

            sport.LogToConsole = log;

            {//https://stackoverflow.com/a/73668856
                sport.Handshake = Handshake.None;
                sport.DtrEnable = true;
                sport.RtsEnable = true;
                sport.StopBits = StopBits.One;
                sport.DataBits = 8;
                sport.Parity = Parity.None;
                sport.ReadBufferSize = 1024 * 1000;//1MB
            }

            sport.Open();

            Port = sport;
            IsConnected = true;

            StopFlag = false;
        }


        public void SetupAdc()
        {
            //var blockSize = blockSize;//samples per block
            var blockCount = 0;
            var bitwidth = UserSettings.BitWidth;
            var sampleRate = (int)UserSettings.SampleRate;

            var activeChannels = this.UserSettings.ChannelIds.Distinct().Count(i => i != Rp2040AdcChannels.None);


            TargetRepository.SampleRate = sampleRate / activeChannels;

            var channels = UserSettings.ChannelIds;

            Stopped = false;

            UpdateCalibration();

            var cmd = AdcConfig.Default();

            {
                //https://github.com/FilipDominec/rp2daq/blob/main/docs/PYTHON_REFERENCE.md#adc
                cmd.channel_mask = (byte)GetChannelMask(channels);
                cmd.blocksize = blockSize;
                cmd.blocks_to_send = (ushort)blockCount;
                cmd.infinite = infiniteBlocks ? (byte)1 : (byte)0;
                cmd.clkdiv = (ushort)(48_000_000 / sampleRate); //rate is 48MHz/clkdiv (e.g. 96 gives 500 ksps; 48000 gives 1000 sps etc.)
            }

            var cmdBin = cmd.ToArray();// StructTools.RawSerialize(cmd);//serialize into 9 byte binary

            var tmp = BitConverter.ToString(cmdBin);

            byte sop = 0x0e;//not sure why sop is 0x0e here!

            Port.Write(new byte[] { sop, 4 }, cmdBin);

            Port.BaseStream.Flush();



            Thread.Sleep(10);
        }

        public void StopAdc()
        {
            if (Port == null)
                return;

            if (!Port.IsOpen )
                return;

            var port = Port;
            Stopped = true;

            var dt = new byte[] { Sop, 12, 1 };//12: ADC_Stop id, 1:https://github.com/FilipDominec/rp2daq/blob/09a00b00b7f1d8f63583e23a4ced26a01f095c3d/include/adc_builtin.c#L100

            port.Write(dt);
            port.BaseStream.Flush();

            do
            {
                var ret = port.ReadAvailable();
                Thread.Sleep(100);

            } while (port.BytesToRead != 0);//read existing data
        }

        public bool TryGetDeviceIdentifier(out string id)
        {
            var dt = new byte[] { 1, 0 };

            var sport = Port;
            sport.Write(dt);

            Thread.Sleep(200);

            var l = 34;

            var t1 = sport.BytesToRead;

            id = null;

            if (t1 != l)
                return false;
                //throw new Exception("Unexpected resonse length, try unplug and replug the PICO");

            var buf = sport.ReadExplicitLength(l);
            var pass = 4;

            var ver = Encoding.ASCII.GetString(buf, pass, buf.Length - pass);
            id= ver;
            return true;
        }

        public Dictionary<byte, bool> LastGpioValues = new Dictionary<uint8_t, bool>();

        public event EventHandler<EventArgs> OnGpioChange;
        public event EventHandler Crashed;

        public void HandleGpioChange(byte pin, bool newValue)
        {
            LastGpioValues[pin] = newValue;

            if (OnGpioChange != null)
                OnGpioChange(this, EventArgs.Empty);
        }

        public bool TryReadGpioChangeData(byte[] data)
        {
            var pin = data[0];
            var newVal = data[4];

            if (newVal != 4 && newVal != 8)
                return false;

            var isPressed = newVal == 8;

            HandleGpioChange(pin, isPressed);

            return true;
        }

        private void DecodeAdcValues(byte[] buff, int[] adcs)
        {
            var arrLength = buff.Length;
            byte a, b, c;
            int v1, v2;
            var arrCnt = 0;


            for (var j = 0; j < arrLength; j += 3)
            {
                a = buff[j + 0];
                b = buff[j + 1];
                c = buff[j + 2];

                v1 = a + ((b & 0xF0) << 4);

                v2 = ((c & 0xF0) >> 4) + ((b & 0x0F) << 4) + ((c & 0x0F) << 8);

                adcs[arrCnt++] = v1;
                adcs[arrCnt++] = v2;
            }
        }


        public void ReadAdcValues12bit(byte[] buff)
        {
            var arrLength = buff.Length;

            byte a, b, c;
            int v1, v2, newVal;
            float volt1, volt2;

            //var arr = TargetRepository.Samples;
            //var arrF = TargetRepository.SamplesF;
            //this is only single channel

            ISampleRepository<int> c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12;

            {
                var lst = new List<ISampleRepository<ushort>>();
                var lst2 = new List<ISampleRepository<int>>();

                var chns = UserSettings.ChannelIds.Distinct().ToList();
                //chns.Remove(Rp2040AdcChannels.None);

                //var cccs = new List<ISampleRepository<int>>();

                {
                    var flags = new bool[5];

                    var enms = new[] {
                        Rp2040AdcChannels.Gpio26,
                        Rp2040AdcChannels.Gpio27,
                        Rp2040AdcChannels.Gpio28,
                        Rp2040AdcChannels.InternalReference,
                        Rp2040AdcChannels.InternalTempratureSensor};


                    for (int i = 0; i < enms.Length; i++)
                    {
                        var flag = flags[i] = UserSettings.ChannelIds.Contains(enms[i]);

                        if (flag)
                        {
                            lst2.Add(TargetRepository.Channels[i]);
                        }
                    }
                }

                var intCount = buff.Length / 3 * 2;

                var ints = ArrayPool.Int32(intCount);

                DecodeAdcValues(buff, ints);

                var arrCnt = 0;

                {
                    var c_ = lst2.Count;

                    for (var i = 0; i < ints.Length; i++)
                    {
                        if (arrCnt == c_)
                            arrCnt = 0;

                        lst2[arrCnt++].Add(ints[i]);
                    }
                }
                
                /*

                if (chns.Count == 0)
                    throw new Exception();
                else if (chns.Count == 1)
                {
                    var cc = TargetRepository.Channels[0];

                    c1 = c2 = c3 = c4 = c5 = c6 = c7 = c8 = c9 = c10 = c11 = c12 = cc;
                }
                else if (chns.Count == 2)
                {
                    var cc1 = TargetRepository.Channels[0];
                    var cc2 = TargetRepository.Channels[1];

                    c1 = c3 = c5 = c7 = c9 = c11 = cc1;
                    c2 = c4 = c6 = c8 = c10 = c12 = cc2;
                }
                else if (chns.Count == 3)
                {
                    var cc1 = TargetRepository.Channels[0];
                    var cc2 = TargetRepository.Channels[1];
                    var cc3 = TargetRepository.Channels[2];

                    c1 = c4 = c7 = c10 = cc1;
                    c2 = c5 = c8 = c11 = cc2;
                    c3 = c6 = c9 = c12 = cc3;
                }
                else if (chns.Count == 4)
                {
                    var cc1 = TargetRepository.Channels[0];
                    var cc2 = TargetRepository.Channels[1];
                    var cc3 = TargetRepository.Channels[2];
                    var cc4 = TargetRepository.Channels[3];

                    c1 = c5 = c9 = cc1;
                    c2 = c6 = c10 = cc2;
                    c3 = c7 = c11 = cc3;
                    c4 = c8 = c12 = cc4;
                }
                else
                {
                    throw new NotImplementedException();
                }

                */
            }

            /*

            if (arrLength % 2 != 0)
                throw new Exception();

            var sampleCount = arrLength / 3 * 2;

            if (sampleCount % 12 != 0)
                throw new Exception();

            var arrs = new ISampleRepository<int>[] { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12 };

            


            for (var i = 0; i < ints.Length; i++)
            {
                if (arrCnt == 12)
                    arrCnt = 0;

                arrs[arrCnt++].Add(ints[i]);

            }

            */
            //double alpha, beta;
            //alpha = 3.3 / 4096;
            //beta = 0;

            //RuntimeVariables.Instance.LastAlphaCh1 = alpha;
            //RuntimeVariables.Instance.LastBetaCh1 = beta;

           

            /*
            for (var j = 0; j < arrLength; j += 3)
            {
                a = buff[j + 0];
                b = buff[j + 1];
                c = buff[j + 2];

                v1 = a + ((b & 0xF0) << 4);

                v2 = ((c & 0xF0) >> 4) + ((b & 0x0F) << 4) + ((c & 0x0F) << 8);

               

                arrs[arrCnt++].Add((ushort)v1);
                arrs[arrCnt++].Add((ushort)v2);

                TotalReads += 2;
            }
            */
        }


        public void SetRefPwm()
        {
            //todo: fixme
            {//pwm config pair

                //RP2040 PWM Frequency and Duty cycle set algorithm.
                //https://medium.com/@pranjalchanda08/rp2040-pwm-frequency-and-duty-cycle-set-algorithm-2eb953b83dd4


                byte gpioPin = 0;
                uint8_t gpio = gpioPin;         // default=0		min=0		max=25
                uint16_t wrap_value = 125_00;        // default=999		min=1		max=65535
                uint16_t clkdiv = 0;            // default=1		min=1		max=255
                uint8_t clkdiv_int_frac = 0;    // default=0		min=0		max=15

                byte[] arr;

                using (var str = new MemoryStream())
                {
                    var rwtr = new BinaryWriter(str);

                    rwtr.Write(Sop);
                    rwtr.Write(5);//pwm config pair
                    rwtr.Write(gpio);
                    rwtr.Write(wrap_value);
                    rwtr.Write(clkdiv);
                    rwtr.Write(clkdiv_int_frac);

                    arr = str.ToArray();
                }

                Port.Write(arr, 0, arr.Length);

                var reportCode = Port.ReadExplicitLength(1)[0];

                if (reportCode != 5)
                    throw new Exception();

            }
        }


        public bool[] GetGpioValues(params byte[] pins)
        {
            var buf = new bool[pins.Length];

            for (int i = 0; i < pins.Length; i++)
            {
                byte pin = pins[i];
                var dt = new byte[] { Sop, 2, pin };

                Port.Write(dt);

                Port.BaseStream.FlushAsync();

                var tt = Port.ReadExplicitLength(3);

                var code = tt[0];

                if (code != 2)
                    throw new Exception();

                var gpio = tt[1];
                var val = tt[2];

                buf[i] = val == 1;
            }

            return buf;
        }

        private double Alpha, Beta;


        public void SetLocalCalibParams()
        {
            var g26_10x = 19;
            var g27_10x = 20;
            var g28_10x = 21;

            /*
            if (UserSettings.ChannelId == Rp2040AdcChannels.Gpio26)
            {
                if (LastGpioValues[g26_10x])//channel1, 10x pressed
                {
                    Alpha = CalibrationData.AlphaA1;
                    Beta = CalibrationData.BetaA1;
                }
                else
                {
                    Alpha = CalibrationData.AlphaB1;
                    Beta = CalibrationData.BetaB1;
                }
            }*/

            Alpha = 1;
            Beta = 0;

            /*

            if (UserSettings.ChannelId == Rp2040AdcChannels.Gpio27)
            {
                if (LastGpioValues[20])//channel1, 10x pressed
                {
                    Alpha = CalibrationData.AlphaA2;
                    Beta = CalibrationData.BetaA2;
                }
                else
                {
                    Alpha = CalibrationData.AlphaB2;
                    Beta = CalibrationData.BetaB2;
                }
            }


            if (UserSettings.ChannelId == Rp2040AdcChannels.Gpio28)
            {
                if (LastGpioValues.ContainsKey(19) && LastGpioValues[19])//channel1, 10x pressed
                {
                    Alpha = CalibrationData.AlphaA2;
                    Beta = CalibrationData.BetaA2;
                }
                else
                {
                    Alpha = CalibrationData.AlphaB2;
                    Beta = CalibrationData.BetaB2;
                }
            }

            */
        }


        public void ReadGpioInitialValues()
        {
            throw new NotImplementedException();
            /*
            var pinsList = new List<byte>();

            foreach (var item in RuntimeVariables.Instance.Channels)
            {
                var x10 = item.Pin10x;
                var acdc = item.PinAcDc;

                pinsList.Add((byte)x10);

                if (acdc != null)
                    pinsList.Add((byte)acdc);
            }

            var vals = GetGpioValues(pinsList.ToArray());

            for (var i = 0; i < pinsList.Count; i++)
            {
                byte pin = pinsList[i];
                var val = vals[i];

                HandleGpioChange(pin, val);
            }
            //HandleGpioChange(gpio, val == 1); not right
            */
        }


        private void UpdateCalibration()
        {
            var cnt = this.GetMaxChannelCount();

            var buf = TargetRepository.CalibrationDatas = new CalibrationInfo[cnt];
            var en = TargetRepository.ChannelEnable = new bool[cnt];

            var chs = new Rp2040AdcChannels[] {
                Rp2040AdcChannels .Gpio26,
                Rp2040AdcChannels .Gpio27,
                Rp2040AdcChannels .Gpio28,
                Rp2040AdcChannels .InternalReference,
                Rp2040AdcChannels .InternalTempratureSensor,
            };

            for (var i = 0; i < buf.Length; i++)
            {
                buf[i] = new CalibrationInfo(1, 0);//todo read from gpio stuff

                en[i] = UserSettings.ChannelIds.Contains(chs[i]);
            }

            
        }

        public void ReadAdcData()
        {
            int bitwidthCurr;
            var sport = Port;

            var bw = UserSettings.BitWidth;//faster access

            var arrLength = blockSize * bw / 8;

            //var arr = TargetRepository.Samples;

            //var arrF = TargetRepository.SamplesF;

            var _4Count = 0;


            StopFlag = false;

            {//reading data

                byte[] buf = new byte[arrLength];

                var cnt = 0;

                var adcHeaderLength = 25;
                var gpioHeaderLength = 16;

                var adcReportHeader = new byte[adcHeaderLength];

                var gpioReportHeader = new byte[gpioHeaderLength];

                //double adc1_alpha = 1.0f;
                //double adc1_beta = 1.0f;   //volt = adc1*adc1_alpha + adc1_beta

                //var tmp = Emptied.Dequeue();

                byte[] buff;

                //byte a, b, c;
                //int v1, v2;

                //Console.WriteLine("Starting ADC read");

                var tmp = new byte[1];

                //float volt1, volt2;

                while (!StopFlag)//read while true
                {
                    var tmpii = TotalReads;

                    //read next block
                    if (sport.BytesToRead == 0)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    byte packageCode;

                    {
                        sport.ReadArray(tmp);
                        packageCode = tmp[0];
                    }

                    //Debug.WriteLine("Package :" + packageCode);

                    //packageCode = 4;

                    if (packageCode == 0x03)//gpio change
                    {
                        sport.ReadArray(gpioReportHeader);

                        if (!TryReadGpioChangeData(gpioReportHeader))
                        {
                            //throw new Exception("gpio parse error");
                            ReConnect();
                            continue;
                        }
                        
                    }


                    else if (packageCode == 0x04)//adc report
                    {
                        //_4Count++;
                        sport.ReadArray(adcReportHeader);//adc_report binary

                        bitwidthCurr = adcReportHeader[2];// report._data_bitwidth;

                        if (bitwidthCurr != bw)
                        {
                            //throw new Exception("Packet Loss! Try Reconnect...");//kind of checking!
                            ReConnect();
                            continue;
                        }
                            

                        buff = buf;

                        sport.ReadArray(buf);

                        ReadAdcValues12bit(buff);
                     
                    }
                    else
                    {
                        ReConnect();
                        //throw new Exception("packet loss");
                    }

                    cnt++;
                }


            }
        }

        private void FlushIncomingData()
        {
            while (Port.BytesToRead != 0)
            {
                Port.ReadAvailable();
            }
        }

        //public event EventHandler<EventArgs> OnPacketLoss;

        public void StartSync()
        {
            Connect(false);
            
            var sport = Port;
           
            ReConnect();

            ReadAdcData();
        }

        private void ReConnect()
        {
            var sport = this.Port;

            {//send command for ADC stop
                StopAdc();
            }

            FlushIncomingData();

            string ver;

            if (!TryGetDeviceIdentifier(out ver))
            {
                throw new Exception("Invalid firmware version");
            }

            //if (!ver.StartsWith("rp2daq_240715"))
            //    throw new Exception("Invalid firmware version");


            {//send command for set ADC pins to high impedance (remove pull up/down resistors), no need anymore, fixed on firmware
                //SetHighZ();
            }

            {//set gpio push button callback
                var pins = new byte[] { 19, 20 };

                foreach (var pin in pins)
                {
                    var cmd = new byte[] { 0x05, 0x03, pin, 0x01, 0x01 };
                    sport.Write(cmd);
                }
            }

            {//read gpio initial values
                //ReadGpioInitialValues();
            }

            {//set calibration parameters
                SetLocalCalibParams();
            }

            {//send command for ADC
                SetupAdc();
            }
        }

        public void DisConnect()
        {
            StopAdc();

            if (Port != null)
                Port.Close();

            Port = null;

            IsConnected = false;
        }


    }
}
