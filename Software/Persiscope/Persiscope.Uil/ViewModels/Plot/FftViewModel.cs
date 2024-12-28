using Persiscope.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Persiscope.Lib;
using System.ComponentModel;
using System.Reflection;
using System;
using Persiscope.Common;
using SkiaSharp;
using Persiscope.Ui.Bitmap;
using System.Threading;
using Avalonia.Threading;
using Avalonia.Rendering;
using System.Linq;
using Avalonia.Controls.Shapes;
using Persiscope.Ui;
using Persiscope.Uil.Views.Plot;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using HarfBuzzSharp;
using Avalonia.Controls;


namespace Persiscope.Uil.ViewModels.Plot
{
    public class FftViewModel : BaseFpsRenderViewModel
    {

        public FftViewModel()
        {
            NeedSignalPropertyCalculation = true;
        }

        public void RenderReset()
        {
            if (BitmapContext == null)
                return;

            throw new Exception();

            //BitmapContext.Value.Clear();
        }

        public void RenderShot(UiDataRepositorySnapshot shot)
        {
            if (BitmapContext == null)
            {
                return;
            }


            var ctx = base.BitmapContext.Value;

            /**/

            //var inf = new SKImageInfo()
            var bmp = Bitmap;// BitmapContext.Value.Bmp;

            using (var lc = bmp.Lock())
            {
                var bmpFl = lc.GetType().GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance);

                var bmpp = (SKBitmap)bmpFl.GetValue(lc);

                //couner++;

                using (var cnv = new SKCanvas(bmpp))
                {
                    cnv.Clear(UiVariables.BackGround);

                    var chs = UiRuntimeVariables.Instance.Channels;

                    foreach (var pkg in shot.Packages)
                    {
                        if (pkg == null)
                            continue;

                        if (!pkg.ChannelInfo.Enabled)
                            continue;



                        var n = pkg.Depth;

                        //if(!pkg.ChannelInfo.Source.CanFillChannelReadouts(shot,))

                        //pkg.ChannelInfo.Source.FillChannelReadouts(shot, currChn);

                        var col = pkg.ChannelInfo.DisplayColor;

                        RenderChannel(cnv, bmpp, pkg.SampleRate, pkg.SignalPropertyList, pkg.Fft, col);
                    }
                }
            }
            /**/
        }



        private void DrawGridsHoriz(SKCanvas cnv, SKBitmap bmp, double fMin, double fMax,Color col)
        {
            var color = new SKColor(col.R, col.G, col.B, col.A); ;

            var Margin = UiVariables.Margin;

            var trsX = Linear1DTransformation.FromInOut(fMin, fMax, Margin.Left, bmp.Width - Margin.Right);

            var count = 10;

            byte r = 128;
            byte b = 128;
            byte g = 0;

            var delta = ((fMax - fMin) * 1.0 / count);

            var dx = fMax - fMin;

            //var ctx = context;

            for (int ii = 0; ii <= count; ii++)
            {
                var y = delta * ii + fMin;

                var yp = (int)trsX.Transform(y);


                var maxX = bmp.Height - Margin.Top;

                for (int i = Margin.Bottom; i < maxX; i++)
                {
                    //BitmapContextExtensions.SetPixel(ctx, (int)yp, i, r, g, b);
                    cnv.DrawPoint(yp, i, color);
                }

                /**/

                var frq = fMin + ii * (fMax - fMin) / count;
                /**/
                var str = FriendlyStringUtil.ToSI(frq, "0.000") + "Hz";


                
                var fontSize = 15;

                var font = new SKFont();

                var formattedText = SKTextBlob.Create(str, font, new SKPoint(10, 10));

                //context.DrawText(formattedText, yp, 10, Colors.White);
                //context.FillText(formattedText, yp, 10, Colors.White);

                using (var paint = new SKPaint())
                {
                    paint.TextSize = 64.0f;
                    paint.IsAntialias = true;
                    paint.Color = color;
                    paint.IsStroke = false;
                    paint.StrokeWidth = 3;
                    paint.TextAlign = SKTextAlign.Left;

                    //canvas.DrawText("Skia", info.Width / 2f, 144.0f, paint);
                    cnv.DrawText(formattedText, yp, 10, paint);
                }

                /**/
                
                /**/
                //WriteableBitmapEx.FillText(bmp, formattedText, 100, 100, Colors.Blue);
            }

            /*

            for (int i = Margin.Top; i < bmp.Height - Margin.Bottom; i++)
            {
                bmp.SetPixel(Margin.Left, i, r, g, b);
                bmp.SetPixel(bmp.Width - Margin.Right, i, r, g, b);
            }

            */

        }


        Linear1DTransformation LastYTransformLin;


        private void RenderChannel(SKCanvas cnv, SKBitmap bmp, double sampleRate, 
            SignalPropertyList lst, FftContext fft, Color col)
        {

            var color = new SKColor(col.R, col.G, col.B, col.A); ;

            var w = bmp.Width;
            var h = bmp.Height;

            var Margin = UiVariables.Margin;

            var minFreq = 0;
            var maxFreq = sampleRate / 2;

            var maxMag = double.MinValue;


            var n = fft.Magnitudes.Length;

            for (var i = 0; i < n; i++)
            {
                if (maxMag < fft.Magnitudes[i])
                    maxMag = fft.Magnitudes[i];
            }

            maxMag = Math.Log10(maxMag);

            var trsX = Linear1DTransformation.FromInOut(minFreq, maxFreq, Margin.Left, w - Margin.Right);
            var trsY = LastYTransformLin = Linear1DTransformation.FromInOut(0, maxMag, h - Margin.Bottom, Margin.Top);


            DrawGridsHoriz(cnv, bmp, minFreq, maxFreq, Colors.White);

            int x, y;

            //var ctx = context;
            //using (;// Bmp2.GetBitmapContext())
            {
                var stFreq = Math.Max(minFreq, 0);
                var enFreq = Math.Min(maxFreq, sampleRate / 2);

                var stId = stFreq * n / sampleRate;
                var enId = enFreq * n / sampleRate;

                for (var i = (int)stId; i < enId; i++)
                {

                    var mag = fft.Magnitudes[i];
                    var freq = i * sampleRate / n;

                    var xi = freq;
                    var yi = Math.Log10(mag);

                    x = (int)trsX.Transform(xi);
                    y = (int)trsY.Transform(yi);


                    

                    if (x > 0 && y > 0 && x < w && y < h)
                        cnv.DrawPoint(x, y, color);
                }
            }
        }


        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override void DeActivate()
        {
            throw new NotImplementedException();
        }

        public override int[] GetActiveChannels()
        {
            throw new NotImplementedException();
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void RenderFrame(UiDataRepositorySnapshot shot)
        {
            RenderShot(shot);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
