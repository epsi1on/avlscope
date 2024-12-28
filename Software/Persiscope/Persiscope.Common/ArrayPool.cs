using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Common
{
    public static class ArrayPool
    {
        public class Pooll<T> where T : struct
        {
            public static int Rents = 0;
            public static int Returns = 0;
            public static int Allocates = 0;



            public T[] Rent(long length)
            {
                lock (Buffer)
                {
                    for (var i = 0; i < Buffer.Count; i++)
                    {
                        var b = Buffer[i];

                        if (b != null)
                            if (b.Length == length)
                            {
                                Buffer[i] = null;
                                Rents++;
                                return b;
                            }
                    }

                }

                Allocates++;
                Rents++;

                return new T[length];
            }

            public void Return(T[] arr)
            {
                var flag = false;

                lock (Buffer)
                {
                    for (var i = 0; i < Buffer.Count; i++)
                    {
                        if (ReferenceEquals(Buffer, arr))
                            throw new Exception();
                    }

                    for (var i = 0; i < Buffer.Count; i++)
                    {
                        var b = Buffer[i];

                        if (b == null)
                        {
                            Buffer[i] = arr;
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                        Buffer.Add(arr);

                    Returns++;
                }
            }

            List<T[]> Buffer = new List<T[]>(32);
        }

        private static Pooll<double> Pdouble = new Pooll<double>();
        private static Pooll<float> Pfloat = new Pooll<float>();
        private static Pooll<short> Pshort = new Pooll<short>();
        private static Pooll<ushort> Pushort = new Pooll<ushort>();
        private static Pooll<int> Pint = new Pooll<int>();
        private static Pooll<long> Plong = new Pooll<long>();
        private static Pooll<bool> Pbool = new Pooll<bool>();
        private static Pooll<byte> Pbyte = new Pooll<byte>();
        private static Pooll<Complex> PComplex = new Pooll<Complex>();


        static void Nearest(ref int a)
        {
            var log = Math.Floor(Math.Log(a, 2));

            var pow = (int)log + 1;

            a = IntPow(2, (uint)pow);
        }

        static int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }



        public static short[] Short(int lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<short>.Shared.Rent(lengh);
            //return new short[lengh];
            return Pshort.Rent(lengh);
        }

        public static ushort[] UShort(int lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<short>.Shared.Rent(lengh);
            //return new short[lengh];
            return Pushort.Rent(lengh);
        }
        public static float[] Float(int lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<short>.Shared.Rent(lengh);
            //return new short[lengh];
            return Pfloat.Rent(lengh);
        }

        public static byte[] Byte(int lengh)
        {
            //Nearest(ref lengh);

            //return ArrayPool<byte>.Shared.Rent(lengh);
            //return new byte[lengh];
            return Pbyte.Rent(lengh);
        }

        public static bool[] Bool(int lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<bool>.Shared.Rent(lengh);
            //return new bool[lengh];
            return Pbool.Rent(lengh);
        }

        public static Complex[] Complex(long lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<bool>.Shared.Rent(lengh);
            //return new bool[lengh];
            return PComplex.Rent(lengh);
        }

        public static double[] Double(long lengh)
        {
            //Nearest(ref lengh);
            //return ArrayPool<double>.Shared.Rent(lengh);
            //return new double[lengh];
            return Pdouble.Rent(lengh);
        }

        public static int[] Int32(long lengh)
        {
            //Nearest(ref lengh);
            //ArrayPool<int>.Shared.Rent(lengh);
            //return new int[lengh];
            return Pint.Rent(lengh);
        }

        public static long[] Long(int lengh)
        {
            //Nearest(ref lengh);
            //ArrayPool<int>.Shared.Rent(lengh);
            //return new int[lengh];
            return Plong.Rent(lengh);
        }

        public static void Return(long[] arr)
        {
            //ArrayPool<int>.Shared.Return(arr);
            Plong.Return(arr);
        }

        public static void Return(Complex[] arr)
        {
            //ArrayPool<int>.Shared.Return(arr);
            PComplex.Return(arr);
        }

        public static void Return(int[] arr)
        {
            //ArrayPool<int>.Shared.Return(arr);
            Pint.Return(arr);
        }

        public static void Return(short[] arr)
        {
            //ArrayPool<short>.Shared.Return(arr);
            Pshort.Return(arr);
        }

        public static void Return(ushort[] arr)
        {
            //ArrayPool<short>.Shared.Return(arr);
            Pushort.Return(arr);
        }


        public static void Return(double[] arr)
        {
            // ArrayPool<double>.Shared.Return(arr);
            Pdouble.Return(arr);
        }

        public static void Return(byte[] arr)
        {
            //ArrayPool<byte>.Shared.Return(arr);
            Pbyte.Return(arr);
        }

        public static void Return(bool[] arr)
        {
            //ArrayPool<bool>.Shared.Return(arr);
            Pbool.Return(arr);
        }

        public static void Return(float[] arr)
        {
            Pfloat.Return(arr);
        }
    }
}
