using Persiscope.Common;
using SharpFFTW;
using SharpFFTW.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public static class FftwUtil
    {
        public static ComplexArray inputC;//pooling
        public static ComplexArray outputC;//pooling

        public static Plan LastPlan;


        public static Mutex FftMutex = new Mutex();

        public static void CalcFftSharp(double[] input, Complex[] output)
        {
            FftMutex.WaitOne();

            var n = input.Length;


            var tt = IntPtr.Size == 8;

            {
                if (n != output.Length)
                    throw new Exception();

                //----

                if (inputC != null)
                    if (inputC.Length != n)
                    {
                        inputC.Dispose();
                        inputC = null;
                    }

                if (inputC == null)
                    inputC = new ComplexArray(n);

                //----

                if (outputC != null)
                    if (outputC.Length != n)
                    {
                        outputC.Dispose();
                        outputC = null;
                    }

                if (outputC == null)
                    outputC = new ComplexArray(n);

            }

            var inputt = inputC;// new ComplexArray(input.Length);
            var outputt = outputC;// new ComplexArray(output.Length);

            var length = input.Length;

            {
                var cpx = ArrayPool.Complex(n);

                for (int i = 0; i < n; i++)
                {
                    cpx[i] = new Complex(input[i], 0);
                }

                inputC.Set(cpx);

                ArrayPool.Return(cpx);
            }

            if (LastPlan != null)
            {
                LastPlan.Dispose();
                LastPlan = null;
            }

            var plan1 = LastPlan = Plan.Create1(length, inputt, outputt, Direction.Forward, Options.Estimate);

            plan1.Execute();

            plan1.Dispose();

            outputC.CopyTo(output);

            outputC.Clear();
            inputC.Clear();

            FftMutex.ReleaseMutex();
        }

        private static void CalcFft(short[] input, Complex[] output)
        {
            /*
            var i1 = ArrayPool.Complex(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                i1[i] = new Complex(input[i], 0);
            }

            using (var pinIn = new PinnedArray<Complex>(i1))
            using (var pinOut = new PinnedArray<Complex>(output))
            {
                DFT.FFT(pinIn, pinOut);
            }

            ArrayPool.Return(i1);
            */
        }


    }
}
