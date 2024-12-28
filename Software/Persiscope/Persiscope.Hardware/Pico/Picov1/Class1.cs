using System;
using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using uint64_t = System.UInt64;
using System.Runtime.InteropServices;
using int8_t = System.Byte;
using System.Windows;
using System.IO;

namespace Persiscope.Hardware.Pico.Picov1
{
    [StructLayout(LayoutKind.Explicit, Size = 24, Pack = 1)]
    public struct ADC_Report
    {
        [FieldOffset(0)]
        public uint8_t report_code;  // again this is 0x04
        [FieldOffset(1)]
        public uint16_t _data_count;
        [FieldOffset(3)]
        public uint8_t _data_bitwidth;
        [FieldOffset(4)]
        public uint64_t start_time_us;
        [FieldOffset(12)]
        public uint64_t end_time_us;
        [FieldOffset(20)]
        public uint8_t channel_mask;
        [FieldOffset(21)]
        public uint16_t blocks_to_send;
        [FieldOffset(23)]
        public uint8_t block_delayed_by_usb;
    }

    //[StructLayout(LayoutKind.Explicit, Size = 9, Pack = 1)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AdcConfig
    {
        public static AdcConfig Default()
        {
            var buf = new AdcConfig();
            //buf.message_type = 0x04;
            buf.channel_mask = 1;
            buf.blocksize = 1000;
            buf.infinite = 0;
            buf.blocks_to_send = 1;
            buf.clkdiv = 96;
            buf.trigger_gpio = 0xff;//default=-1		min=-1		max=24 GPIO number for start trigger (set to -1 to make ADC start w/o trigger)
            //-1 int int8_t is 0xff in hex
            return buf;
        }



        public byte[] ToArray()
        {
            //acording to https://github.com/FilipDominec/rp2daq/blob/09a00b00b7f1d8f63583e23a4ced26a01f095c3d/include/adc_builtin.c#L49

            using (var str = new MemoryStream())
            {
                var rwtr = new BinaryWriter(str);

                rwtr.Write(channel_mask);

                rwtr.Write(blocksize);
                rwtr.Write(infinite);

                rwtr.Write(blocks_to_send);
                rwtr.Write(clkdiv);
                //rwtr.Write(start_time_us);
                //rwtr.Write(end_time_us);
                //rwtr.Write(waits_for_usb);
                //rwtr.Write(waits_for_trigger);

                //rwtr.Write(block_delayed_by_usb);
                rwtr.Write(trigger_gpio);
                rwtr.Write(trigger_on_falling_edge);

                rwtr.Flush();

                return str.ToArray();
            }
        }

        public uint8_t channel_mask;

        public uint16_t blocksize;
        public uint8_t infinite;
        public uint32_t blocks_to_send;
        public uint16_t clkdiv;
        //public uint64_t start_time_us;
        //public uint64_t end_time_us;
        //public uint8_t waits_for_usb;
        //public uint8_t waits_for_trigger;
        //public uint8_t block_delayed_by_usb;
        public int8_t trigger_gpio;
        public uint8_t trigger_on_falling_edge;



        //[FieldOffset(0)]
        //public uint8_t channel_mask;       // default=1		min=0		max=31 Masks 0x01, 0x02, 0x04 are GPIO26, 27, 28; mask 0x08 internal reference, 0x10 temperature sensor

        //[FieldOffset(1)]
        //public uint8_t infinite;           // default=0		min=0		max=1  Disables blocks_to_send countdown (reports keep coming until explicitly stopped)

        //[FieldOffset(2)]
        //public uint16_t blocksize;         // default=1000		min=1		max=8192 Number of sample points until a report is sent

        //public UInt32
        //[FieldOffset(5)]
        //public uint16_t blocks_to_send;    // default=1		min=0		         Number of reports to be sent (if not infinite)
        //[FieldOffset(7)]
        //public uint16_t clkdiv;            // default=96		min=96		max=65535 Sampling rate is 48MHz/clkdiv (e.g. 96 gives 500 ksps; 48000 gives 1000 sps etc.)

    }

    [StructLayout(LayoutKind.Explicit, Size = 6, Pack = 1)]
    public struct PwmConfigPairCommand
    {
        [FieldOffset(0)]
        uint8_t gpio;               // default=0		min=0		max=25
        [FieldOffset(1)]
        uint16_t wrap_value;        // default=999		min=1		max=65535
        [FieldOffset(3)]
        uint16_t clkdiv;            // default=1		min=1		max=255
        [FieldOffset(5)]
        uint8_t clkdiv_int_frac;	// default=0		min=0		max=15
    }




    //https://stackoverflow.com/a/14125895
    public static class StructTools
    {
        /// <summary>
        /// converts byte[] to struct
        /// </summary>
        public static T RawDeserialize<T>(byte[] rawData, int position)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length - position)
                throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
            nint buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            T retobj = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }

        /// <summary>
        /// converts a struct to byte[]
        /// </summary>
        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            nint buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }
    }
}
