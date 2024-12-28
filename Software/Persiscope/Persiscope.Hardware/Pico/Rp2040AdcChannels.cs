using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Hardware.Pico
{
    [Flags]
    public enum Rp2040AdcChannels : uint
    {
        //channel_mask : Masks 0x01, 0x02, 0x04 are GPIO26, 27, 28; mask 0x08 internal reference, 0x10 temperature sensor
        None = 0,
        Gpio26 = 1,
        Gpio27 = 2,
        Gpio28 = 4,
        InternalReference = 8,
        InternalTempratureSensor = 16,
    }
}
