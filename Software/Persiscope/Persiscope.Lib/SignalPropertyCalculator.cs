using System;
using System.Linq;

namespace Persiscope.Lib
{
    public class SignalPropertyCalculator
    {

        static System.Diagnostics.Stopwatch wc = new System.Diagnostics.Stopwatch();

        public static SignalPropertyList Calculate(double[] adcValues, FftContext context,double sampleRate)
        {
            var ys = adcValues;

            //throw new Exception();

            var alpha = double.NaN;// RuntimeVariables.Instance.LastAlphaCh1;
            var beta = double.NaN;//RuntimeVariables.Instance.LastBetaCh1;
            //var sampleRate = RuntimeVariables.AdcConfig.SampleRate;

            var buf = new SignalPropertyList();

            buf.alpha = alpha;
            buf.beta = beta;

            {
                var min = double.MaxValue;
                var max = double.MinValue;
                double sum = 0;

                double y;

                for (var i = 0; i < ys.Length; i++)
                {
                    y = ys[i];

                    if (y > max) max = y;
                    if (y < min) min = y;
                    sum += y;
                }

                buf.Min = min;
                buf.Max = max;
                buf.Avg = (double)sum / ys.Length;
            }


            if (buf.Min == buf.Max)
            {
                buf.Frequency = 0;

                return buf;
            }

            long[] histogram;
            long histogramSum;
            long tmp;

            {
                {
                    
                }

                /*
                {
                    histogram = ArrayPool.Long(4096);

                    for (int i = 0; i < ys.Length; i++)
                    {
                        tmp = ys[i];
                        histogram[ys[i]]++;
                    }

                    histogramSum = histogram.Sum();

                    buf.MinPercentile1 = CalculateLowPercentile(histogram, histogramSum, 0.1);
                    buf.MaxPercentile1 = CalculateHighPercentile(histogram, histogramSum, 0.1);

                    buf.MinPercentile5 = CalculateLowPercentile(histogram, histogramSum, 5);
                    buf.MaxPercentile5 = CalculateHighPercentile(histogram, histogramSum, 5);

                    buf.Domain = (ushort)(buf.Max - buf.Min);
                    buf.Percentile1Domain = (ushort)(buf.MaxPercentile1 - buf.MinPercentile1);
                    buf.Percentile5Domain = (ushort)(buf.MaxPercentile5 - buf.MinPercentile5);
                }
                */


                //wc.Restart();
                //buf.FftContext = context;
                //wc.Stop();

                //Log.Info("FFT took {0} ms", wc.ElapsedMilliseconds);

                double freq, shiftRadian;

                FftFrequencyDetectorUtil.GetStrongestFrequency(context, sampleRate, out freq, out shiftRadian);

                buf.Frequency = freq;
                buf.PhaseRadian = shiftRadian;

                {
                    var avg = (buf.MinPercentile1 + buf.MaxPercentile1) / 2.0;

                    long less = 0;
                    long greater = 0;

                    /*
                    for (int i = 0; i < histogram.Length; i++)
                    {
                        if (i < avg)
                            less += histogram[i];

                        if (i > avg)
                            greater += histogram[i];
                    }

                    buf.PwmDutyCycle = greater / (double)(less + greater);
                    */
                }
            }
            return buf;
        }


        public static ushort CalculateLowPercentile(long[] hist, long histSum, double percentile)
        {
            var tot = histSum;// hist.Sum();

            var tmp = 0l;

            if (hist.Length > short.MaxValue)
                throw new Exception();


            for (ushort i = 0; i < hist.Length; i++)
            {
                tmp += hist[i];

                if (tmp / (double)tot > 0.01 * percentile)
                {
                    return i;
                }
            }


            return 0;
        }

        public static ushort CalculateHighPercentile(long[] hist, long histSum, double percentile)
        {
            var tot = histSum;// hist.Sum();

            var tmp = 0l;

            if (hist.Length > short.MaxValue)
                throw new Exception();


            for (ushort i = (ushort)(hist.Length - 1); i >= 0; i--)
            {
                tmp += hist[i];

                if (tmp / (double)tot > 0.01 * percentile)
                {
                    return i;
                }
            }


            return 0;
        }
    }
}
