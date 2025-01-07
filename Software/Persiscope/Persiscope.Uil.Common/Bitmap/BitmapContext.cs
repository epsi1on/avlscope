using Avalonia.Media.Imaging;
using Persiscope.Ui.Bitmap;
//using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public struct BitmapContext
    {

        public WriteableBitmap Bmp;

        public readonly int width;
        public readonly int height;
        public readonly nint pixelsPtr;
        public readonly uint[] pixels;
        public readonly int pixelsCount;


        /// <summary>
        /// Creates an instance of a BitmapContext, with specified ReadWriteMode
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride">in bytes</param>
        public BitmapContext(IntPtr bitmap, int width, int height, int stride)
        {
            this.width = width;
            this.height = height;
            this.pixelsPtr = bitmap;
            var stint = stride / 4;
            this.pixelsCount = width * stint;
            
            /*
            unsafe
            {
                throw new Exception();
                //pixels = new Span<uint>(bitmap.ToPointer(), 4);
            }

            */
        }



        public static BitmapContext FromBitmap(WriteableBitmap bmp)
        {
            return BitmapFactory.FromWriteableBitmap(bmp);
        }
    }
}
