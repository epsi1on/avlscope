using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Persiscope.Lib;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Ui.Bitmap
{
    public static class BitmapFactory
    {
        public static WriteableBitmap New(int width, int height)
        {
            var fmt = PixelFormat.Bgra8888;

            return new WriteableBitmap(new Avalonia.PixelSize(width, height), new Avalonia.Vector(96, 96), fmt, AlphaFormat.Opaque);
        }


        public static BitmapContext FromWriteableBitmap(WriteableBitmap bmp)
        {
            var buf = new BitmapContext();

            buf.Bmp = bmp;

            return buf;

            var lc  = bmp.Lock();

            var type = typeof(Avalonia.Skia.Helpers.PixelFormatHelper).Assembly.GetType("Avalonia.Skia.WriteableBitmapImpl+BitmapFramebuffer");
            var hnd = type.GetField("Address", System.Reflection.BindingFlags.Instance);

            throw new NotImplementedException();
        }
    }
}
