using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public static class FftFrequencyDetectorUtil
    {
        public static void GetKStrongestFrequency(FftContext context, double sampleRate, int k, out double[] freqs, out double[] phases)
        {
            var highest = HpVectorOperation.KHighest(context.Magnitudes, k);

            freqs = new double[k];
            phases = new double[k];

            
            throw new NotImplementedException();
        }

        public static void GetStrongestFrequency(FftContext context, double sampleRate, out double freq, out double phase)
        {

            var max = double.MinValue;
            var maxId = -1;

            var mags = context.Magnitudes;
            var n = mags.Length;

            for (var i = 1; i < mags.Length; i++)//skip 0, dc voltage
            {
                if (mags[i] > max)
                    max = mags[maxId = i];
            }

            freq = maxId * sampleRate / (double)n ;
            phase = context.Context[maxId].Phase;
        }
    }
}
