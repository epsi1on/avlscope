using System;
using System.Runtime.Serialization;

namespace Persiscope.Hardware
{
    [Serializable]
    public abstract class BaseDeviceUserSettingsData 
    {
        //public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        public abstract int GetAdcSampleRate();

        //gets the record depth, some cases it is fixed (like span sampling, sampling 100k points then stop a while)
        //some cases it is variable (like continues sampling)
        public abstract long GetRecordDepth();
    }
}
