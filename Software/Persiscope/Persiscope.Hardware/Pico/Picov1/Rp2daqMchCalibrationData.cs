using Persiscope.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persiscope.Hardware.Pico.Picov1
{
    [Serializable]
    public class Rp2daqMchCalibrationData : BaseDeviceCalibrationData
    {
        [JsonInclude]
        public double AlphaA1, AlphaA2;
        [JsonInclude]
        public double AlphaB1, AlphaB2;
        [JsonInclude]
        public double AlphaC1, AlphaC2;
        [JsonInclude]
        public double BetaA1, BetaA2;
        [JsonInclude]
        public double BetaB1, BetaB2;
        [JsonInclude]
        public double BetaC1, BetaC2;

       
        public Rp2daqMchCalibrationData()
        {

        }

       
    }
}
