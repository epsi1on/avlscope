using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Persiscope.Lib;
using Persiscope.Ui;
using Persiscope.Ui.Bitmap;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.ViewModels.Plot
{
    public abstract partial class BaseFpsRenderViewModel:ViewModelBase
    {
        static bool DoLog = false;

        protected BaseFpsRenderViewModel()
        {
            //Loaded += OnLoaded;

            //Unloaded += OnUnloaded;
        }

        public event EventHandler<EventArgs> RenderShotDone;



        private Thread renderThread;

        public void OnLoaded(object? sender, RoutedEventArgs e)
        {
            renderThread = new Thread(RenderLoopSync);
            renderThread.Name = "Render Thread";
            renderThread.Start();
            //InvokeState = 0;

            if (DoLog)
                Log.DebugInfo("Renderer Loaded {0}", GetType().Name);
        }


        //private int InvokeState = 0;

        public void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            RenderLoopFlag = false;

            //while (InvokeState != 2)
            //    Thread.Sleep(1);

            //InvokeState = 2;

            while (renderThread.ThreadState == System.Threading.ThreadState.Running)
                Thread.Sleep(1);


            //renderThread.Join();

            if (DoLog)
                Log.DebugInfo("Renderer Unloaded {0}", GetType().Name);
        }

        //for case like EKG render which need high speed, calculating signal properties like frequency, min max etc is useless and reduces the speed
        public bool NeedSignalPropertyCalculation { get; protected set; }

        public bool NeedSamplesResort { get; protected set; }

        public abstract void Activate();
        public abstract void DeActivate();
        public abstract void Init();
        public abstract void Reset();

        public void UpdateImageSize(int w, int h)
        {
            /**/
            RenderMutex.WaitOne();

            try
            {
                Bitmap = BitmapFactory.New(w, h);
                BitmapContext = BitmapFactory.FromWriteableBitmap(_bitmap);
            }
            finally
            {
                RenderMutex.ReleaseMutex();
            }

            /**/


        }


        public abstract int[] GetActiveChannels();

        //public abstract void RenderFrame(DataRepositorySnapshot shot);

        public abstract void RenderFrame(UiDataRepositorySnapshot shot);


        private bool RenderLoopFlag;

        public static Mutex RenderMutex = new Mutex();


        public void RenderLoopSync()
        {
            RenderLoopFlag = true;

            var tm = Stopwatch.StartNew();

            while (true)
            {
                if (!RenderLoopFlag)
                    break;

                Thread.Sleep(10);


                var fps = UiRuntimeVariables.RenderFrameRate;

                
                
                //var chs = UiRuntimeVariables.Instance.Channels;

                
                //var channels = ; GetActiveChannels();

                //throw new NotImplementedException();
                tm.Restart();

                if (DoLog)
                    Log.Info("render start at 0 ms");

                var rateFrameMs = 1 / fps * 1000;//how many milis between two consecutive frames


                using (var snapshot =
                    DataRepositorySnapshot.GenerateSnapshot(LibRuntimeVariables.Instance.CurrentRepo))
                {
                    if (DoLog)
                        Log.Info("generate data snapshot at {0} ms", tm.ElapsedMilliseconds);
                    //tm.Restart();


                    var chns = UiRuntimeVariables.Instance.Channels;

                    var flag = NeedSignalPropertyCalculation;

                   // if (!UiDataRepositorySnapshot.CanGenerate(snapshot, chns))
                   //     continue;//just on startaup, wait for ready

                    using var shot2 = UiDataRepositorySnapshot.Generate(snapshot, chns, flag);

                    if (DoLog)
                        Log.Info("generate UI data snapshot at {0} ms", tm.ElapsedMilliseconds);

                    //tm.Restart();

                    if (DoLog)
                        Log.DebugInfo("Locking");
                    RenderMutex.WaitOne();
                    if (DoLog)
                        Log.DebugInfo("Locked");

                    try
                    {
                        RenderFrame(shot2);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }

                    if (DoLog)
                        Log.DebugInfo("Unlocking");
                    RenderMutex.ReleaseMutex();
                    if (DoLog)
                        Log.DebugInfo("Unlocked");

                    //if (InvokeState != 2)
                    {
                        //InvokeState = 1;

                        if (RenderShotDone != null)
                            RenderShotDone.Invoke(this, null);

                        //InvokeState = 2;
                    }

                    tm.Stop();

                    if (DoLog)
                        Log.Info("Render frame took {0} ms", tm.ElapsedMilliseconds);

                    var wait = rateFrameMs - tm.ElapsedMilliseconds;

                    if (wait > 0)
                        Thread.Sleep((int)wait);
                }
            }
        }

        [ObservableProperty]
        private WriteableBitmap _bitmap;

        [ObservableProperty]
        public BitmapContext? _bitmapContext;

        

        

    }
}
