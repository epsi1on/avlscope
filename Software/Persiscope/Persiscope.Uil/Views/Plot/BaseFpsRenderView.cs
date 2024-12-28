using Avalonia.Controls;
using Avalonia.Interactivity;
using Persiscope.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Persiscope.Ui;
using Persiscope.Uil.ViewModels.Plot;
using Avalonia.Markup.Xaml;
using Persiscope.Ui.Bitmap;
using Avalonia.Threading;

namespace Persiscope.Uil.Views.Plot
{
    public abstract class BaseFpsRenderView : UserControl
    {

        public BaseFpsRenderViewModel Context;

        protected Image GetImage()
        {
            return this.FindControl<Image>("image");
        }

        private void BaseFpsRenderView_DataContextChanged(object? sender, EventArgs e)
        {

            if (Context != null)
                return;

            Context = this.DataContext as BaseFpsRenderViewModel;

            Context.RenderShotDone += Context_RenderShotDone;
        }


        public Image lastImage;

        private void Context_RenderShotDone(object? sender, EventArgs e)
        {
            Dispatcher.UIThread.Invoke(() =>

            {
                if (lastImage == null)
                    lastImage = GetImage();
                lastImage.InvalidateVisual();
            });
        }

        protected BaseFpsRenderView() : base()
        {
            DataContextChanged += BaseFpsRenderView_DataContextChanged;

            Loaded += OnLoaded;

            Unloaded += OnUnloaded;

            this.SizeChanged += BaseFpsRenderView_SizeChanged;

        }

        private void BaseFpsRenderView_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            var sz = sender as BaseFpsRenderView;

            Context.UpdateImageSize((int)e.NewSize.Width, (int)e.NewSize.Height);

        }


        /**/

        private Thread renderThread;
        private bool RenderLoopFlag;

        protected void OnLoaded(object? sender, RoutedEventArgs e)
        {
            Context.OnLoaded(this, e);

           // renderThread = new Thread(RenderLoopSync);
           // renderThread.Start();

            Log.DebugInfo("Renderer Loaded {0}", GetType().Name);


            var img = this.GetImage();

            var w = img.Bounds.Right - img.Bounds.Left;
            var h = img.Bounds.Bottom - img.Bounds.Top;

        }

        protected void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            Context.OnUnloaded(this, e);

            //RenderLoopFlag = false;

            //renderThread.Join();

            Log.DebugInfo("Renderer Unloaded {0}", GetType().Name);
        }

        /*
        //for case like EKG render which need high speed, calculating signal properties like frequency, min max etc is useless and reduces the speed
        public bool NeedSignalPropertyCalculation { get; protected set; }

        public bool NeedSamplesResort { get; protected set; }

        public abstract void Activate();
        public abstract void DeActivate();
        public abstract void Init();
        public abstract void Reset();


        public abstract int[] GetActiveChannels();

        //public abstract void RenderFrame(DataRepositorySnapshot shot);

        public abstract void RenderFrame(UiDataRepositorySnapshot shot);

        

        

        public readonly Mutex RenderMutex = new Mutex();


        public void RenderLoopSync()
        {
            RenderLoopFlag = true;

            var tm = Stopwatch.StartNew();

            while (true)
            {
                if (!RenderLoopFlag)
                    return;

                Thread.Sleep(10);


                var fps = LibRuntimeVariables.RenderFramerate;

                var channels = GetActiveChannels();

                //throw new NotImplementedException();
                tm.Restart();

                var rateFrameMs = 1 / fps * 1000;//how many milis between two consecutive frames


                using (var snapshot =
                    DataRepositorySnapshot.GenerateSnapshot(LibRuntimeVariables.Instance.CurrentRepo, channels))
                {
                    var chns = UiRuntimeVariables.Instance.Channels;

                    var flag = NeedSignalPropertyCalculation;

                    if (!UiDataRepositorySnapshot.CanGenerate(snapshot, chns))
                        continue;//just on startaup, wait for ready

                    using var shot2 = UiDataRepositorySnapshot.Generate(snapshot, chns, flag);

                    Log.Info("cals signal props took {0} ms", tm.ElapsedMilliseconds);

                    tm.Restart();

                    Log.DebugInfo("Locking");
                    RenderMutex.WaitOne();
                    Log.DebugInfo("Locked");

                    try
                    {
                        RenderFrame(shot2);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }

                    Log.DebugInfo("Unlocking");
                    RenderMutex.ReleaseMutex();
                    Log.DebugInfo("Unlocked");


                    tm.Stop();

                    var wait = rateFrameMs - tm.ElapsedMilliseconds;

                    Log.Info("Render frame took {0} ms", tm.ElapsedMilliseconds);

                    if (wait > 0)
                        Thread.Sleep((int)wait);
                }
            }
        }

        */
    }
}
