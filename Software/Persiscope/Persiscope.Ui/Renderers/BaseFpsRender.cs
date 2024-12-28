using Avalonia.Controls;
using AvlScope.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvlScope.Ui.Renderers
{
    public abstract class BaseFpsRender : UserControl
    {
        //for case like EKG render which need high speed, calculating signal properties like frequency, min max etc is useless and reduces the speed
        public bool NeedSignalPropertyCalculation { get; protected set; }

        public bool NeedSamplesResort{ get; protected set; }

        public abstract void Activate();
        public abstract void DeActivate();
        public abstract void Init();
        public abstract void Reset();


        public abstract int[] GetActiveChannels();

        //public abstract void RenderFrame(DataRepositorySnapshot shot);

        public abstract void RenderFrame(UiDataRepositorySnapshot shot);


        private bool RenderLoopFlag;

        public readonly Mutex RenderMutex = new Mutex();

        protected BaseFpsRender()
        {
        }

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

                var channels = this.GetActiveChannels();

                //throw new NotImplementedException();
                tm.Restart();

                var rateFrameMs = (1 / fps) * 1000;//how many milis between two consecutive frames


                using (var snapshot = 
                    DataRepositorySnapshot.GenerateSnapshot(LibRuntimeVariables.Instance.CurrentRepo, channels))
                {
                    var chns = UiRuntimeVariables.Instance.Channels;

                    var flag = this.NeedSignalPropertyCalculation;

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
                        

                        this.RenderFrame(shot2);

                        
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
    }
}
