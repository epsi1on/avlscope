using Avalonia.Media.Imaging;
using Avalonia.Media;
using AvlScope.Lib;
using System.ComponentModel;
using System.Reflection;
using System;
using AvlScope.Common;
using SkiaSharp;
using AvlScope.Ui.Bitmap;
using System.Threading;
using Avalonia.Threading;
using Avalonia.Rendering;
using System.Linq;
using Avalonia.Controls.Shapes;

namespace AvlScope.Ui.Renderers.Ekg;

public class EkgContextClass : INotifyPropertyChanged
{
    #region INotifyPropertyChanged members and helpers

    public event PropertyChangedEventHandler PropertyChanged;

    protected static bool AreEqualObjects(object obj1, object obj2)
    {
        var obj1Null = ReferenceEquals(obj1, null);
        var obj2Null = ReferenceEquals(obj2, null);

        if (obj1Null && obj2Null)
            return true;

        if (obj1Null || obj2Null)
            return false;

        if (obj1.GetType() != obj2.GetType())
            return false;

        if (ReferenceEquals(obj1, obj2))
            return true;

        return obj1.Equals(obj2);
    }

    protected void OnPropertyChanged(params string[] propertyNames)
    {
        if (propertyNames == null)
            return;

        if (PropertyChanged != null)
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Bitmap Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public WriteableBitmap Bitmap
    {
        get { return _Bitmap; }
        set
        {
            if (AreEqualObjects(_Bitmap, value))
                return;

            var _fieldOldValue = _Bitmap;

            _Bitmap = value;

            OnBitmapChanged(this, new PropertyValueChangedEventArgs<WriteableBitmap>(_fieldOldValue, value));

            OnPropertyChanged("Bitmap");
        }
    }

    private WriteableBitmap _Bitmap;

    public EventHandler<PropertyValueChangedEventArgs<WriteableBitmap>> BitmapChanged;

    public static void OnBitmapChanged(object sender, PropertyValueChangedEventArgs<WriteableBitmap> e)
    {
        var obj = sender as EkgContextClass;

        if (obj.BitmapChanged != null)
            obj.BitmapChanged(obj, e);
    }

    #endregion

    #region BitmapContext Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public BitmapContext? BitmapContext
    {
        get { return _BitmapContext; }
        set
        {
            if (AreEqualObjects(_BitmapContext, value))
                return;

            var _fieldOldValue = _BitmapContext;

            _BitmapContext = value;

            OnBitmapContextChanged(this, new PropertyValueChangedEventArgs<BitmapContext?>(_fieldOldValue, value));

            OnPropertyChanged("BitmapContext");
        }
    }

    private BitmapContext? _BitmapContext;

    public EventHandler<PropertyValueChangedEventArgs<BitmapContext?>> BitmapContextChanged;

    public static void OnBitmapContextChanged(object sender, PropertyValueChangedEventArgs<BitmapContext?> e)
    {
        var obj = sender as EkgContextClass;

        if (obj.BitmapContextChanged != null)
            obj.BitmapContextChanged(obj, e);
    }

    #endregion


    public EkgRenderer Parent;

    public void RenderReset()
    {
        if (BitmapContext == null)
        {
            return;
        }


        throw new Exception();

        //BitmapContext.Value.Clear();
    }

    static Random random = new Random();

    static SKColor[] cl = new SKColor[2] { SKColors.Blue, SKColors.Red };

    int couner = 0;

  


    public void RenderShot(UiDataRepositorySnapshot shot)
    {
        if (BitmapContext == null)
        {
            //return;
        }


        var ctx = _BitmapContext.Value;

        /**/

        //var inf = new SKImageInfo()
        var bmp = this.Bitmap;// BitmapContext.Value.Bmp;

        using (var lc = bmp.Lock())
        {
            var bmpFl = lc.GetType().GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance);

            var bmpp = (SKBitmap)bmpFl.GetValue(lc);

            couner++;

            using (var cnv = new SKCanvas(bmpp))
            {
                cnv.Clear(UiVariables.BackGround);

                var chs = UiRuntimeVariables.Instance.Channels;

                foreach (var pkg in shot.Packages)
                {
                    if (!pkg.ChannelInfo.Enabled)
                        continue;

                    var n = pkg.Depth;

                    //pkg.ChannelInfo.Source.FillChannelReadouts(shot, currChn);

                    var col = pkg.ChannelInfo.DisplayColor;

                    RenderChannel(cnv, bmpp, pkg.SampleRate, pkg.Values, pkg.StartIndex, col);
                }

            }
        }
        /**/
    }


    Linear1DTransformation LastYTrans;

    private void RenderChannel(SKCanvas cnv, SKBitmap bmp, double sampleRate, double[] ys, long startIdx, Color color)
    {
        var tk = UiVariables.Margin;

        var tx = Linear1DTransformation.FromInOut(0, ys.Length, tk.Left, bmp.Width - tk.Right);

        var ty = Linear1DTransformation.FromInOut(0, 4096, bmp.Height - tk.Bottom, tk.Top);

        var st = startIdx - ys.Length;

        using var pt = new SKPaint();

        pt.Color = new SKColor(color.R, color.G, color.B, color.A);


        var y2s = ArrayPool.Double(ys.Length);

        {
            var L = y2s.Length;
            var idx = startIdx;
            var t = (int)(L-startIdx % L);

            var thisArr = ys;
            var other = y2s;

            var sz = sizeof(double);

            {
                Buffer.BlockCopy(thisArr, (sz * t), other, 0, sz * (L - t));

                Buffer.BlockCopy(thisArr, 0, other, sz * (L - t), sz * t);
            }

        }

        for (var i = 0; i < y2s.Length; i++)
        {
            var x = i;
            var y = y2s[i];

            var x_ = (float)tx.Transform(x);
            var y_ = (float)ty.Transform(y);


            var x2_ = tx.TransformBack(x_ + 1);

            if (x2_ >= y2s.Length)
                x2_ = y2s.Length - 1;

            var min = double.MaxValue;
            var max = double.MinValue;


            /**/
            for (var j = (int)x; j < x2_; j++)
            {
                if (y2s[j] > max)
                    max = y2s[j];

                if (y2s[j] < min)
                    min = y2s[j];
            }
            i = (int)x2_;

            var min_ = (float)ty.Transform(min);
            var max_ = (float)ty.Transform(max);


            if (Math.Abs(min_ - max_) < 1)
                min_ = max_ + 1;

            cnv.DrawLine(new SKPoint(x_, min_), new SKPoint(x_, max_), pt);

            /*
            

            */
            //cnv.DrawPoint(new SKPoint(x_, y_), color);
        }

        {
            var x = (float)tx.Transform(startIdx % y2s.Length);

            cnv.DrawLine(new SKPoint(x, 0), new SKPoint(x, bmp.Height), pt);
        }


        ArrayPool.Return(y2s);
        return;
    }


    private void RenderChannel(SKCanvas cnv, SKBitmap bmp, double sampleRate, CalibrationInfo calib, int[] ys, long idx, SignalPropertyList prp, SKColor color)
    {
        var tk = UiVariables.Margin;

        /**/
        var tx = Linear1DTransformation.FromInOut(0, ys.Length, tk.Left, bmp.Width - tk.Right);

        var ty = Linear1DTransformation.FromInOut(0, 4096, tk.Bottom, bmp.Height - tk.Top);


        var st = idx - ys.Length;

        var pt = new SKPaint();
        pt.Color = color;

        for (var i = 0; i < ys.Length; i++)
        {
            var x = i;
            var y = ys[i];

            var x_ = (float)tx.Transform(x);
            var y_ = (float)ty.Transform(y);

            
            var x2_ = tx.TransformBack(x_ + 1);

            if (x2_ >= ys.Length)
                x2_ = ys.Length - 1;

            var min = int.MaxValue;
            var max = int.MinValue;


            /**/
            for (var j = (int)x; j < x2_; j++)
            {
                if (ys[j] > max)
                    max = ys[j];

                if (ys[j] < min)
                    min = ys[j];
            }
            i = (int)x2_;
            
            var min_ = (float)ty.Transform(min);
            var max_ = (float)ty.Transform(max);
            


            cnv.DrawLine(new SKPoint(x_, min_), new SKPoint(x_, max_), pt);

            /*
            

            */
            //cnv.DrawPoint(new SKPoint(x_, y_), color);
        }

        {
            var x = (float)tx.Transform(idx % ys.Length);

            cnv.DrawLine(new SKPoint(x, 0), new SKPoint(x, bmp.Height), pt);
        }

        pt.Dispose();
        return;

        /** /
        var deltaT = 1.0 / sampleRate;


        var w = bmp.Width;
        var h = bmp.Height;

        var l = ys.Length;

        var freq = prp.Frequency;
        var phase = prp.PhaseRadian;

        var waveLength = 1 / freq;

        var sp = 0;// Parent.CyclesToShow;// cycles to show

        var twl = sp * waveLength;

        var min = calib.Transform(ys.Min());
        var max = calib.Transform(ys.Max());

        var Margin = UiVariables.Margin;


        var trsX = Linear1DTransformation.FromInOut(0, twl, Margin.Left, w - Margin.Right);
        var trsY = LastYTrans = Linear1DTransformation.FromInOut(min, max, h - Margin.Bottom, Margin.Top);

        {
            var st = 0;
            var en = l;// lamdaCount * drawWindowCount;

            var oCnt = Parent.NumberOfCyclesToRender;//oCnt x oscilations

            {
                var cnt2 = Parent.MinPointsToRender / (sampleRate * waveLength);

                if (cnt2 > oCnt)
                    oCnt = (int)cnt2;
            }

            var samples = oCnt * waveLength * sampleRate;

            st = 0;
            en = (int)samples;

            if (en > l)
                en = l;

            var shiftSec = phase / (2 * Math.PI) * waveLength;

            {
                float x, y;

                for (var i = st; i < en; i++)
                {
                    var xi = i * deltaT;// xs[i];

                    xi = xi + shiftSec;//% twl;

                    while (xi < 0)
                        xi += twl;

                    xi = xi % twl;

                    var ty = calib.Transform(ys[i]);

                    x = (float)trsX.Transform(xi);
                    y = (float)trsY.Transform(ty);

                    if (x > 0 && y > 0 && x < w && y < h)
                        cnv.DrawPoint(x, y, color);
                }
            }

        }

        /**/
    }

    public void UpdateImageSize(int w, int h)
    {
        Parent.RenderMutex.WaitOne();

        try
        {
            this.Bitmap = BitmapFactory.New(w, h);
            this.BitmapContext = BitmapFactory.GetBitmapContext(Bitmap);
        }
        finally
        {
            Parent.RenderMutex.ReleaseMutex();
        }
    }


}