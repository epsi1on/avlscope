using Persiscope.Lib;
using Persiscope.Common;
using System;

namespace Persiscope.Ui
{
    public class AnalogSamplePackage : IDisposable
    {
        public double[] Values;//in volt

        //public bool Enabled;

        public double SampleRate;//in sps

        public int Depth;//memory depth, length of Values

        public long StartIndex;//starting index of samples, tip of circular array

        public FftContext Fft;

        public SignalPropertyList SignalPropertyList;

        public ChannelInfo ChannelInfo;




        public void Dispose()
        {
            if (Values != null)
                ArrayPool.Return(Values);

            if(Fft != null) 
                Fft.Dispose();

            Values = null;
        }

        public static AnalogSamplePackage FromPool(long l, double sampleRate)
        {
            var arr = ArrayPool.Double(l);

            var buf = new AnalogSamplePackage();

            buf.SampleRate = sampleRate;
            buf.Values = arr;

            return buf;
        }
    }


}
