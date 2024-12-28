using Persiscope.Ui;
using CommunityToolkit.Mvvm.ComponentModel;
using Persiscope.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Persiscope.Uil.Views.Plot;
using System.Threading;
using Avalonia.Threading;
using Avalonia.Rendering;
using System.Linq;
using Persiscope.Ui;

namespace Persiscope.Uil.ViewModels.Plot
{
    public partial class HarmonicViewModel : BaseFpsRenderViewModel
    {
        public HarmonicViewModel()
        {
            this.NumberOfCyclesToRender = 5;
            this.MinPointToShow = 1000;
            NeedSignalPropertyCalculation = true;
        }

        /**/
        public override void RenderFrame(UiDataRepositorySnapshot shot)
        {
            if (BitmapContext == null)
            {
                //return;
            }

            //if (shot.CalibrationDatas == null)
            //    return;

            var ctx = this.BitmapContext.Value;

            var bmp = this.Bitmap;// BitmapContext.Value.Bmp;


            using (var lc = bmp.Lock())
            {
                var bmpFl = lc.GetType().GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance);

                var bmpp = (SKBitmap)bmpFl.GetValue(lc);

                //couner++;

                using (var cnv = new SKCanvas(bmpp))
                {
                    cnv.Clear(UiVariables.BackGround);


                    //foreach(var ch in UiRuntimeVariables.Instance.Channels)
                    foreach (var pkg in shot.Packages)
                    {
                        //var enables = Enumerable.Repeat(false, LibRuntimeVariables.MaxChannelCount).ToArray();
                        //var colors = Enumerable.Repeat(SKColors.White, LibRuntimeVariables.MaxChannelCount).ToArray();

                        //colors[0] = SKColors.White;
                        //colors[1] = SKColors.Red;
                        //colors[2] = SKColors.Green;
                        //colors[3] = SKColors.Cyan;

                        //var chs = Parent.GetActiveChannels();

                        //foreach (var ch in chs)
                        //    enables[ch] = true;

                        {
                            //for (var i = 0; i < enables.Length; i++)
                            {
                                //  var chn = i;

                                if (pkg == null)
                                    continue;

                                if (!pkg.ChannelInfo.Enabled)
                                    continue;

                                var n = pkg.Depth;

                                //if(!pkg.ChannelInfo.Source.CanFillChannelReadouts(shot,))

                                //pkg.ChannelInfo.Source.FillChannelReadouts(shot, currChn);

                                var col = pkg.ChannelInfo.DisplayColor;

                                //var calib = shot.CalibrationDatas[chn];
                                //var prp = shot.SignalProperties[chn];



                                RenderChannel(cnv, bmpp, pkg.SampleRate, (CalibrationInfo)null, pkg.Values, pkg.SignalPropertyList, col);
                            }
                        }
                    }
                }
            }


            
        }


        private void RenderChannel(SKCanvas cnv, SKBitmap bmp, double sampleRate, CalibrationInfo calib, double[] ys, SignalPropertyList prp, Color col)
        {

            var color = new SKColor(col.R, col.G, col.B, col.A); ;

            var tk = UiVariables.Margin;
            
            var deltaT = 1.0 / sampleRate;


            var w = bmp.Width;
            var h = bmp.Height;

            var l = ys.Length;

            var freq = prp.Frequency;
            var phase = prp.PhaseRadian;

            var waveLength = 1 / freq;

            var sp = this.NumberOfCyclesToRender;// Parent.CyclesToShow;// cycles to show

            var twl = sp * waveLength;

            var min = ys.Min();// calib.Transform(ys.Min());
            var max = ys.Max();// calib.Transform(ys.Max());

            var Margin = UiVariables.Margin;


            var trsX = Linear1DTransformation.FromInOut(0, twl, Margin.Left, w - Margin.Right);
            var trsY = Linear1DTransformation.FromInOut(min, max, h - Margin.Bottom, Margin.Top);

            {
                var st = 0;
                var en = l;// lamdaCount * drawWindowCount;

                var oCnt = this.NumberOfCyclesToRender;//oCnt x oscilations

                {
                    var cnt2 = this.MinPointToShow / (sampleRate * waveLength);

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

                        var ty = (ys[i]);

                        x = (float)trsX.Transform(xi);
                        y = (float)trsY.Transform(ty);

                        if (x > 0 && y > 0 && x < w && y < h)
                            cnv.DrawPoint(x, y, color);
                    }
                }

            }

            
        }

        /**/

        [ObservableProperty]
        private int _numberOfCyclesToRender;

        [ObservableProperty]
        private int _minPointToShow;

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


        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
