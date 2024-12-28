using Avalonia.Controls;
using Persiscope.Common;
using Persiscope.Hardware;
using Persiscope.Hardware.Pico.Picov1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.UiHardware.Rp2040Raw
{
    public class Manager : Persiscope.Uil.Common.BaseConnectManager
    {
        public override Control GetConnectionControl()
        {
            var buf = new Connect();

            

            return buf;
        }

        public override string GetName()
        {
            return "Rpi Pico: Rp2Daq RAW";
        }

        public override string GetDescription()
        {
            //using a 'Raspberry Pi Pico V1`, and opensource `RP2DAQ` firmware (use google to find)
            return "a Rpi Pico board, without any extra components for attenuation/amplification large/small signals.\r\nAnd opensource `RP2DAQ` firmware (use google to find)";
        }

        //public override bool TryCalibrate(out BaseDeviceCalibrationData config)
        //{
        //    throw new NotImplementedException();
        //}

        public override IDaqInterface GenerateDaqInterface(
            BaseDeviceCalibrationData calibrationData,
            BaseDeviceUserSettingsData userSettings)
        {

            var set = userSettings as Rp2daqMchUserSettings;
            var cal = calibrationData as Rp2daqMchCalibrationData;

            var buf = new Rp2DaqMchInterface(set, cal);

            return buf;
        }

        public override BaseDaqConfigGUIControl GenerateUiInterface(BaseDeviceUserSettingsData config)
        {
            var buf = new Rp2040Raw.Connect();

            buf.Init();
            buf.SetDefaultUserSettings(config);

            return buf;
        }

        public override string GetUid()
        {
            return "rp2daq-raw-j40f";
        }

        public override Type GetUserSettingsType()
        {
            return typeof(Rp2daqMchUserSettings);
        }

        protected override BaseDeviceUserSettingsData GetDefaultUserSettings()
        {
            var buf = new Hardware.Pico.Picov1.Rp2daqMchUserSettings();

            buf.ChannelIds = new Hardware.Pico.Rp2040AdcChannels[] 
            {
                Hardware.Pico.Rp2040AdcChannels.Gpio27,
                Hardware.Pico.Rp2040AdcChannels.Gpio28
            };

            buf.SampleRate = 400_000;
            buf.RecordDepthSecs = 0.1;
            buf.BitWidth = 12;

            return buf;
        }

        protected override BaseDeviceCalibrationData GetDefaulCalibration()
        {
            var buf = new Rp2daqMchCalibrationData();

            var vMax = 3.3;


            buf.AlphaA1 = buf.AlphaA2 =
            buf.AlphaB1 = buf.AlphaB2 =
            buf.AlphaC1 = buf.AlphaC2 = vMax / 4096;

            buf.BetaA1 = buf.BetaB1 = buf.BetaC1 = 0;
            buf.BetaA2 = buf.BetaB2 = buf.BetaC2 = 0;

            return buf;
        }
    }
}
