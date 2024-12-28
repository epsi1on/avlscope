using Avalonia.Controls;
using AvlScope.Hardware.Pico;
using AvlScope.Hardware.Pico.Picov1;
using AvlScope.Lib;
using AvlScope.Ui;
using AvlScope.Ui.Renderers.Harmonic;
using System.ComponentModel;
using System.Threading;

namespace AvlScope.DevUi
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.ctrl.Content = rndr = new AvlScope.Uil.MainView();

            //this.DataContext = this.Context = new MainWindowContex() { Parent = this };
            //Context.Connect();
        }


        public UserControl rndr;

        MainWindowContex Context;

    }

    public class MainWindowContex : INotifyPropertyChanged
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

            if (this.PropertyChanged != null)
                foreach (var propertyName in propertyNames)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public MainWindow Parent;

        public readonly int Datarate = 400_000;

        Rp2DaqMchInterface CurrDaq;


        private void StartDaq()
        {
            var sr = 400_000;

            var calib = new Rp2daqMchCalibrationData();
            var sett = new Rp2daqMchUserSettings()
            {
                ComPortName = "Com10",
                BitWidth = 12,
                SampleRate = sr,

                ChannelIds = new Rp2040AdcChannels[] {
                        Rp2040AdcChannels.Gpio26,
                        Rp2040AdcChannels.Gpio27,
                        Rp2040AdcChannels.Gpio28
                    }
            };

            CurrDaq = new Rp2DaqMchInterface(sett, calib);


            LibRuntimeVariables.Instance.CurrentRepo.Init(Datarate, sr, 4);

            CurrDaq.TargetRepository = LibRuntimeVariables.Instance.CurrentRepo;

            //RuntimeVariables.Instance.CurrentRepo
            var thr = new Thread(CurrDaq.StartSync);
            thr.Start();
        }

        private void StartRender()
        {
            //var thr = new Thread(Parent.rndr.RenderLoopSync);
            //thr.Start();
        }

        public void Connect()
        {
            StartDaq();
            StartRender();


        }
    }
}