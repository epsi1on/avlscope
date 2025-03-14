﻿
namespace SharpFFTW.Double
{
    using System;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native array of complex (2*8-byte) floating point numbers.
    /// </summary>
    /// <remarks>
    /// The native memory is managed by FFTW (malloc, free).
    /// </remarks>
    public class ComplexArray : AbstractArray<double>
    {
        private const int SIZE = 16; // sizeof(Complex)

        /// <summary>
        /// Creates a new array of complex numbers.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public ComplexArray(int length)
            : base(length)
        {
            Handle = NativeMethods.fftw_malloc(Length * SIZE);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of doubles.
        /// </summary>
        /// <param name="data">Array of doubles, alternating real and imaginary.</param>
        public ComplexArray(double[] data)
            : this(data.Length / 2)
        {
            Set(data);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from an array of complex numbers.
        /// </summary>
        /// <param name="data">Array of complex numbers.</param>
        public ComplexArray(Complex[] data)
            : this(data.Length)
        {
            Set(data);
        }

        /// <inheritdoc />
        public override void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                NativeMethods.fftw_free(Handle);
                Handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Set the data to an array of complex numbers.
        /// </summary>
        /// <param name="source">Array of doubles, alternating real and imaginary.</param>
        public override void Set(double[] source)
        {
            int size = 2 * Length;

            if (source.Length < size)
            {
                throw new ArgumentException("Array length mismatch.", nameof(source));
            }

            Marshal.Copy(source, 0, Handle, size);
        }

        /// <summary>
        /// Set the data to an array of complex numbers.
        /// </summary>
        /// <param name="source">Array of complex numbers.</param>
        public void Set(Complex[] source)
        {
            if (source.Length != Length)
            {
                throw new ArgumentException("Array length mismatch.", nameof(source));
            }

            var temp = GetTemporaryData(2 * Length);

            for (int i = 0; i < source.Length; i++)
            {
                temp[2 * i] = source[i].Real;
                temp[2 * i + 1] = source[i].Imaginary;
            }

            Marshal.Copy(temp, 0, Handle, Length * 2);
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var temp = GetTemporaryData(2 * Length);

            Array.Clear(temp, 0, temp.Length);

            Marshal.Copy(temp, 0, Handle, Length * 2);
        }

        /// <summary>
        /// Copy data to array of complex number.
        /// </summary>
        /// <param name="target">Array of complex numbers.</param>
        public void CopyTo(Complex[] target)
        {
            if (target.Length < Length)
            {
                throw new ArgumentException("Array length mismatch.", nameof(target));
            }

            var temp = GetTemporaryData(2 * Length);

            CopyTo(temp);

            for (int i = 0; i < Length; i++)
            {
                target[i] = new Complex(temp[2 * i], temp[2 * i + 1]);
            }
        }

        /// <summary>
        /// Copy data to array of doubles.
        /// </summary>
        /// <param name="target">Array of doubles, alternating real and imaginary.</param>
        public override void CopyTo(double[] target)
        {
            int size = 2 * Length;

            if (target.Length < size)
            {
                throw new ArgumentException("Array length mismatch.", nameof(target));
            }

            Marshal.Copy(Handle, target, 0, size);
        }

        /// <summary>
        /// Copy data to array of doubles.
        /// </summary>
        /// <param name="target">Array of doubles, alternating real and imaginary.</param>
        /// <param name="real">If true, only real part is considered.</param>
        public void CopyTo(double[] target, bool real)
        {
            if (!real)
            {
                CopyTo(target);
                return;
            }

            var temp = GetTemporaryData(2 * Length);

            CopyTo(temp);

            for (int i = 0; i < Length; i++)
            {
                target[i] = temp[2 * i];
            }
        }

        /// <summary>
        /// Get data as doubles.
        /// </summary>
        /// <returns>Array of doubles, alternating real and imaginary.</returns>
        public override double[] ToArray()
        {
            int size = 2 * Length;

            double[] data = new double[size];

            Marshal.Copy(Handle, data, 0, size);

            return data;
        }
    }
}
