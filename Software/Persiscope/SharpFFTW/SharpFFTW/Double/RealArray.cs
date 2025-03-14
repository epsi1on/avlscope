﻿
namespace SharpFFTW.Double
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native array of 8-byte floating point numbers.
    /// </summary>
    /// <remarks>
    /// The native memory is managed by FFTW (malloc, free).
    /// </remarks>
    public class RealArray : AbstractArray<double>
    {
        private const int SIZE = 8; // sizeof(double)

        /// <summary>
        /// Creates a new array of doubles.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public RealArray(int length)
            : base(length)
        {
            Handle = NativeMethods.fftw_malloc(Length * SIZE);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of doubles.
        /// </summary>
        /// <param name="data">Array of 8-byte floating point numbers.</param>
        public RealArray(double[] data)
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

        /// <inheritdoc />
        public override void Set(double[] source)
        {
            int size = Length;

            if (source.Length < size)
            {
                throw new ArgumentException("Array length mismatch.", nameof(source));
            }

            Marshal.Copy(source, 0, Handle, size);
        }

        /// <inheritdoc />
        public override void Clear()
        {
            var temp = GetTemporaryData(Length);

            Array.Clear(temp, 0, temp.Length);

            Marshal.Copy(temp, 0, Handle, Length);
        }

        /// <summary>
        /// Copy data to array of doubles.
        /// </summary>
        /// <param name="target">The target array.</param>
        public override void CopyTo(double[] target)
        {
            int size = Length;

            if (target.Length < size)
            {
                throw new ArgumentException("Array length mismatch.", nameof(target));
            }

            Marshal.Copy(Handle, target, 0, size);
        }

        /// <summary>
        /// Get data as doubles.
        /// </summary>
        /// <returns>Array of doubles.</returns>
        public override double[] ToArray()
        {
            int size = Length;

            double[] data = new double[size];

            Marshal.Copy(Handle, data, 0, size);

            return data;
        }
    }
}
