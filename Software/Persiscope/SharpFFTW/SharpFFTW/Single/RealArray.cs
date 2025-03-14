﻿
namespace SharpFFTW.Single
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native array of 4-byte floating point numbers.
    /// </summary>
    /// <remarks>
    /// The native memory is managed by FFTW (malloc, free).
    /// </remarks>
    public class RealArray : AbstractArray<float>
    {
        private const int SIZE = 4; // sizeof(float)

        /// <summary>
        /// Creates a new array of floats.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public RealArray(int length)
            : base(length)
        {
            Handle = NativeMethods.fftwf_malloc(Length * SIZE);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of floats.
        /// </summary>
        /// <param name="data">Array of 4-byte floating point numbers.</param>
        public RealArray(float[] data)
            : this(data.Length)
        {
            Set(data);
        }

        /// <inheritdoc />
        public override void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                NativeMethods.fftwf_free(Handle);
                Handle = IntPtr.Zero;
            }
        }

        /// <inheritdoc />
        public override void Set(float[] source)
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
        /// Copy data to array of floats.
        /// </summary>
        /// <param name="target">The target array.</param>
        public override void CopyTo(float[] target)
        {
            int size = Length;

            if (target.Length < size)
            {
                throw new ArgumentException("Array length mismatch.", nameof(target));
            }

            Marshal.Copy(Handle, target, 0, size);
        }

        /// <summary>
        /// Get data as floats.
        /// </summary>
        /// <returns>Array of floats.</returns>
        public override float[] ToArray()
        {
            int size = Length;

            float[] data = new float[size];

            Marshal.Copy(Handle, data, 0, size);

            return data;
        }
    }
}
