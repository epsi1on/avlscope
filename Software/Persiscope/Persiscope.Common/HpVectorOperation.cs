using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public static class HpVectorOperation
    {

        public static void HpClear<T>(this T[] array) where T : struct
        {
            var def = default(T);

            for (var i = array.Length - 1; i >= 0; i--)
            {
                array[i] = def;
            }
        }

        /*
        public static double KahanSum(double[] x, int n)
        {
            var buf = new KahanSum();

            for (int i = 0; i < n; i++)
            {
                buf.Add(x[i]);
            }

            return x.Sum();
        }
        */

        /*
        /// <summary>
        /// adds single value(a) to all members of x
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="n"></param>
        public static void HpPlus(double[] x, double a, int n)
        {
            var buf = new KahanSum();

            for (int i = 0; i < n; i++)
            {
                x[i] += a;
            }
        }

        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">values need to have a histogram</param>
        /// <param name="n">number of values</param>
        /// <param name="histogram">to be filled with method</param>
        /// <param name="walls">to be filled with method</param>
        public static void HpGenerateHistogram(double[] values, int n, int[] histogram, out double dx, out double x0)
        {
            var max = values.Max();
            var min = values.Min();

            var w = max - min;

            var m = histogram.Length;

            for (var i = 0; i < n; i++)
            {
                var val = values[i];

                var grp = (val - min) / (max - min) * m;

                var g = (int)Math.Floor(grp);

                if (g == m)
                    g--;

                histogram[g]++;

            }

            x0 = min;

            dx = w / m;
        }

        public static void HpCalculateMagnitudeAndPhase(Complex[] context, double[] mgs, double[] phs)
        {
            var n = context.Length;

            if (mgs.Length != n || phs.Length != n)
                throw new Exception();

            //TODO: optimize with SIMD, or unroll or other stuff
            for (var i = 0; i < n; i++)
            {
                mgs[i] = context[i].Magnitude;
                phs[i] = context[i].Phase;
            }
        }

        public static int[] KHighest(this double[] array,int k)
        {
            throw new NotImplementedException();
        }

        public static void AxB(int[] a, double x, double b, double[] res)
        {
            for (var i = res.Length - 1; i >= 0; i--)
            {
                res[i] = a[i] * x + b;
            }
        }
    }
}
