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
    public class Rp2daqMchUserSettings : BaseDeviceUserSettingsData
    {
        [JsonInclude]
        public double RecordDepthSecs;

        [JsonInclude]
        public int SampleRate;

        [JsonInclude]
        public string ComPortName;

        [JsonInclude]
        public Rp2040AdcChannels[] ChannelIds;

        [JsonInclude]
        public int BitWidth;



        public Rp2daqMchUserSettings()
        {
        }

       

        public override int GetAdcSampleRate()
        {
            return SampleRate;
        }

        public override long GetRecordDepth()
        {
            var chCount = ChannelIds.Length;

            return (long)(this.RecordDepthSecs * SampleRate / chCount);
        }

        
    }
}
