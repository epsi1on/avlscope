using AvlScope.Lib;
using System;

namespace AvlScope.Ui
{
    public class AnalogSamplePackage : IDisposable
    {
        public double[] Values;//in volt

        public double SampleRate;//in sps

        public int Depth;//memory depth, length of Values

        public long StartIndex;//starting index of samples, tip of circular array

        public FftContext Context;

        public SignalPropertyList SignalPropertyList;

        public ChannelInfo ChannelInfo;




        public void Dispose()
        {
            if (Values != null)
                ArrayPool.Return(Values);

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
