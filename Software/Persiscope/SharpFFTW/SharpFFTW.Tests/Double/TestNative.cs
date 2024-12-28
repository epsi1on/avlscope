﻿
namespace SharpFFTW.Tests.Double
{
    using NUnit.Framework;
    using SharpFFTW;
    using SharpFFTW.Double;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Test native FFTW interface (1D).
    /// </summary>
    public class TestNative
    {
        [Test]
        public void Test()
        {
            const int SIZE = 8192;

            Assert.True(Example1(SIZE));
            Assert.True(Example2(SIZE));
        }

        bool Example1(int length)
        {
            Console.Write("Test 1: complex transform ... ");

            // This example will show how to work with FFTW using unmanaged memory for
            // input and output. Since we don't have direct access to the native memory,
            // we need to allocate at least one more managed array to copy data between
            // managed and unmanaged memory.

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two unmanaged arrays, properly aligned.
            IntPtr pin = NativeMethods.fftw_malloc(size * sizeof(double));
            IntPtr pout = NativeMethods.fftw_malloc(size * sizeof(double));

            // Create managed input arrays, possibly misaligned.
            var fin = Util.GenerateSignal(size);

            // Copy managed input array to unmanaged input array.
            Marshal.Copy(fin, 0, pin, size);

            // Create test transforms (forward and backward).
            IntPtr plan1 = NativeMethods.fftw_plan_dft_1d(length, pin, pout, Direction.Forward, Options.Estimate);
            IntPtr plan2 = NativeMethods.fftw_plan_dft_1d(length, pout, pin, Direction.Backward, Options.Estimate);

            NativeMethods.fftw_execute(plan1); // Forward.
            NativeMethods.fftw_execute(plan2); // Backward.

            // Clear input array (technically not necessary).
            Array.Clear(fin, 0, fin.Length);

            // Copy unmanaged output of back-transform to managed array (overwriting input array).
            Marshal.Copy(pin, fin, 0, size);

            // Check and see how we did.
            bool success = Util.CheckResults(length, length, fin);

            // Don't forget to free the memory after finishing.
            NativeMethods.fftw_free(pin);
            NativeMethods.fftw_free(pout);
            NativeMethods.fftw_destroy_plan(plan1);
            NativeMethods.fftw_destroy_plan(plan2);

            return success;
        }

        bool Example2(int length)
        {
            Console.Write("Test 2: complex transform ... ");

            // This example will show how to work with FFTW using only managed memory for
            // input and output.

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two managed arrays, possibly misaligned.
            var fin = Util.GenerateSignal(size);
            var fout = new double[size];

            // Get handles and pin arrays so the GC doesn't move them
            var hin = GCHandle.Alloc(fin, GCHandleType.Pinned);
            var hout = GCHandle.Alloc(fout, GCHandleType.Pinned);

            // Get pointers to pinned array.
            IntPtr min = hin.AddrOfPinnedObject();
            IntPtr mout = hout.AddrOfPinnedObject();

            // Create test transforms (forward and backward).
            IntPtr plan1 = NativeMethods.fftw_plan_dft_1d(length, min, mout, Direction.Forward, Options.Estimate);
            IntPtr plan2 = NativeMethods.fftw_plan_dft_1d(length, mout, min, Direction.Backward, Options.Estimate);

            NativeMethods.fftw_execute(plan1);

            // Clear input array and try to refill it from a backwards FFT.
            Array.Clear(fin, 0, size);

            NativeMethods.fftw_execute(plan2);

            // Check and see how we did.
            bool success = Util.CheckResults(length, length, fin);

            // Don't forget to free the memory after finishing.
            NativeMethods.fftw_destroy_plan(plan1);
            NativeMethods.fftw_destroy_plan(plan2);

            hin.Free();
            hout.Free();

            return success;
        }
    }
}