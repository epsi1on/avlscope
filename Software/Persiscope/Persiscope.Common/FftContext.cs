using Persiscope.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public class FftContext : IDisposable
    {
        public double[] Magnitudes;
        public double[] Phases;
        public Complex[] Context;


        public static FftContext FromPool(int length)
        {
            var n = length;

            var ctx = ArrayPool.Complex(n);
            var ph = ArrayPool.Double(n);
            var mag = ArrayPool.Double(n);

            Array.Clear(ctx, 0, n);
            Array.Clear(ph, 0, n);
            Array.Clear(mag, 0, n);

            //FftwUtil.CalcFftSharp(signal, ctx);

            var buf = new FftContext();
            buf.Context = ctx;
            buf.Magnitudes = mag;
            buf.Phases = ph;
            //buf.Update();

            return buf;
        }

        /*
        public static FftContext FromPoolThenSignal(ushort[] signal)
        {
            var n = signal.Length;

            var buf = FromPool(n);

            FftwUtil.CalcFftSharp(signal, buf.Context);

            buf.UpdateArrays();

            return buf;
        }
        */

        public void UpdateArrays()
        {
            var n = Context.Length;

            int sz;

            unsafe
            {
                sz = sizeof(Complex);
            }

            var context = Context;

            var mgs = Magnitudes;
            var phs = Phases;

            HpVectorOperation.HpCalculateMagnitudeAndPhase(context, mgs, phs);
        }

        public void Dispose()
        {
            ArrayPool.Return(Context);
            ArrayPool.Return(Magnitudes);
            ArrayPool.Return(Phases);

            Context = null;
            Magnitudes = null;
            Phases = null;
        }
    }
}
