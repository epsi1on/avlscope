using Avalonia.Media;
using Persiscope.Common;
using Persiscope.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Ui
{
    public class UiRuntimeVariables
    {


        public static readonly string VersionPrefix = "Pre-alpha";
        public static readonly string AppName = "Persiscope";
        public static readonly string AppDescription = "A software based osciloscope.";


        public static readonly UiRuntimeVariables Instance;
        public static readonly double RenderFrameRate = 100;


        public Thread DaqReaderThread;


        public UiRuntimeVariables() { }

        public ChannelInfo[] Channels;

        public bool IsConnected { get; private set; }

        public bool IsCrashed;

        public event EventHandler<bool> IsConnectedChanged;

        public double MaxFps;



        private void SetIsConnected(bool value,object sender = null)
        {
            if (IsConnected == value)
                return;

            IsConnected = value;

            if (IsConnectedChanged != null)
                IsConnectedChanged.Invoke(sender, value);
        }

        static UiRuntimeVariables()
        {
            Instance = new UiRuntimeVariables();

            Instance.Channels = new ChannelInfo[5];


            Instance.Channels[0] = new ChannelInfo() { DisplayColor = Colors.Red, Enabled = true, Id = "Ch1", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 0 } };

            Instance.Channels[1] = new ChannelInfo() { DisplayColor = Colors.Blue, Enabled = true, Id = "Ch2", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 1 } };

            Instance.Channels[2] = new ChannelInfo() { DisplayColor = Colors.Cyan, Enabled = true, Id = "Ch3", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 2 } };

            Instance.Channels[3] = new ChannelInfo() { DisplayColor = Colors.Magenta, Enabled = true, Id = "Ch4", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 3 } };

            Instance.Channels[4] = new ChannelInfo() { DisplayColor = Colors.Gray, Enabled = true, Id = "Ch5", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 4 } };

        }


        public void StartDaqThread(IDaqInterface intfs)
        {
            StopDaqThread();

            DaqReaderThread = new Thread(StartDaqSync);
            DaqReaderThread.Name = "DAQ thread";
            DaqReaderThread.Start(lastInterface = intfs);
        }

        public void StopDaqThread()
        {
            if (lastInterface != null)
                lastInterface.StopFlag = true;

            if (DaqReaderThread != null)
            {
                DaqReaderThread.Join();
            }

            if (lastInterface != null)
            {
                lastInterface.StopAdc();
                lastInterface.DisConnect();
            }



            SetIsConnected(false);
        }

        IDaqInterface lastInterface;

        private void StartDaqSync(object intfs)
        {
            var fce = (IDaqInterface)intfs;

            SetIsConnected(true);
            IsCrashed = false;

            try
            {
                fce.StartSync();
            }
            catch (Exception ex)
            {
                Log.Error("DAQ StartSync() exception: {0}", ex.Message);
                IsCrashed = true;
            }
            finally
            {
                SetIsConnected(false);
                try
                {
                    fce.DisConnect();
                }
                catch 
                { 
                }
            }

        }

    }

   

}
