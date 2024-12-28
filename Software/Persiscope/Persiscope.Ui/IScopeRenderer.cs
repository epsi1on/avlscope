using AvlScope.Lib;
using System;
using System.Windows;

namespace SimpleOsciloscope.Lib
{
    public struct IntRectangle
    {
        public int X, Y, H, W;
    }

    public interface IScopeRenderer
    {

        IntRectangle DoRender(BitmapContext context, SignalPropertyList props);


        //RgbBitmap Render();

        void Init(BitmapContext context);


        void Zoom(BitmapContext context,double delta, int x, int y);


        void ReSetZoom(BitmapContext context);

        string GetPointerValue(BitmapContext context, double x, double y);


        void Activate(BitmapContext context);

        void Deactivate(BitmapContext context);

        //void Clear(bool enabled);
    }
}