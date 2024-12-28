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

namespace AvlScope.Ui.Renderers.Harmonic;

public class HarmonicContextClass : INotifyPropertyChanged
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
        var obj = sender as HarmonicContextClass;

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
        var obj = sender as HarmonicContextClass;

        if (obj.BitmapContextChanged != null)
            obj.BitmapContextChanged(obj, e);
    }

    #endregion


    public HarmonicRenderer Parent;

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

        /** /

        if (shot.CalibrationDatas == null)
            return;

        var ctx = _BitmapContext.Value;

        var bmp = this.Bitmap;// BitmapContext.Value.Bmp;

        using (var lc = bmp.Lock())
        {
            var bmpFl = lc.GetType().GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance);

            var bmpp = (SKBitmap)bmpFl.GetValue(lc);

            couner++;

            using (var cnv = new SKCanvas(bmpp))
            {
                cnv.Clear(UiVariables.BackGround);

                var enables = Enumerable.Repeat(false, LibRuntimeVariables.MaxChannelCount).ToArray();
                var colors = Enumerable.Repeat(SKColors.White, LibRuntimeVariables.MaxChannelCount).ToArray();

                colors[0] = SKColors.White;
                colors[1] = SKColors.Red;
                colors[2] = SKColors.Green;
                colors[3] = SKColors.Cyan;

                var chs = Parent.GetActiveChannels();

                foreach (var ch in chs)
                    enables[ch] = true;

                {
                    for (var i = 0; i < enables.Length; i++)
                    {
                        var chn = i;

                        if (!enables[i])
                            continue;

                        var col = colors[i];

                        var calib = shot.CalibrationDatas[chn];
                        var prp = shot.SignalProperties[chn];

                        RenderChannel(cnv, bmpp, shot.SampleRate, calib, shot.ChannelsSnapshot[chn], prp, col);
                    }
                }
            }
        }


        /**/
    }

    public void RenderShot(DataRepositorySnapshot shot)
    {
        if (BitmapContext == null)
        {
            //return;
        }


        if (shot.CalibrationDatas == null)
            return;

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

                var enables = Enumerable.Repeat(false, LibRuntimeVariables.MaxChannelCount).ToArray();
                var colors = Enumerable.Repeat(SKColors.White, LibRuntimeVariables.MaxChannelCount).ToArray();

                colors[0] = SKColors.White;
                colors[1] = SKColors.Red;
                colors[2] = SKColors.Green;
                colors[3] = SKColors.Cyan;

                var chs = Parent.GetActiveChannels();

                foreach (var ch in chs)
                    enables[ch] = true;

                {
                    for (var i = 0; i < enables.Length; i++)
                    {
                        var chn = i;

                        if (!enables[i])
                            continue;

                        var col = colors[i];

                        var calib = shot.CalibrationDatas[chn];
                        var prp = shot.SignalProperties[chn];

                        RenderChannel(cnv, bmpp, shot.SampleRate, calib, shot.ChannelsSnapshot[chn], prp, col);
                    }
                }
            }
        }

        //Thread.Sleep(100);

        //

        //throw new Exception();


        /**/

        //bmp.SetPixels(ctx.pixelsPtr);
        
        //ctx.Clear(RuntimeVariables.Instance.ImageBackgroundColor);
        /*
        var w = ctx.Width;
        var h = ctx.Height;

        ctx.DrawLine(random.Next(0, w), random.Next(0, h), random.Next(0, w), random.Next(0, h), Colors.Black);

        var msg = string.Format("{0}, {1}", w, h);

        var fc = new Typeface("default");

        var ft = new FormattedText(msg, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, fc, 50, Brushes.Black, null);

        ctx.DrawText(ft, 0, 0, Colors.Red);

        this.Bitmap.AddDirtyRectThreadSafe(new Int32Rect(0, 0, ctx.Width, ctx.Height));
        */

        /**/
    }



    Linear1DTransformation LastYTrans;

    private void RenderChannel(SKCanvas cnv, SKBitmap bmp, double sampleRate, CalibrationInfo calib, int[] ys, SignalPropertyList prp, SKColor color)
    {
        var tk = UiVariables.Margin;
        /**/
        var deltaT = 1.0 / sampleRate;


        var w = bmp.Width;
        var h = bmp.Height;

        var l = ys.Length;

        var freq = prp.Frequency;
        var phase = prp.PhaseRadian;

        var waveLength = 1 / freq;

        var sp = Parent.CyclesToShow;// cycles to show

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