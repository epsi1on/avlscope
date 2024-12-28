using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Persiscope.Common;
using Persiscope.Hardware;
using Persiscope.Lib;
using Persiscope.Ui;
using Persiscope.Uil.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.ViewModels
{
    public partial class ConnectViewModel : ViewModelBase
    {

        public ConnectViewModel()
        {
            Persiscope.UiHardware.Connections.Initfdint();

            _availableConnectionManagers = Persiscope.UiHardware.Connections.GetManagers();

            this.PropertyChanged += ConnectViewModel_PropertyChanged;

            this.IsConnected = UiRuntimeVariables.Instance.IsConnected;

            UiRuntimeVariables.Instance.IsConnectedChanged += Instance_IsConnectedChanged;
        }

        private void Instance_IsConnectedChanged(object? sender, bool e)
        {
            var cn = this.IsConnected = UiRuntimeVariables.Instance.IsConnected;

            if(!cn && UiRuntimeVariables.Instance.IsCrashed)//likely crashed
            {
                try
                {
                    new Thread(DisConnect).Start();
                }
                catch(Exception ex) 
                {
                    Log.Error("DAQ DisConnect() after crash exception: {0}", ex.Message);
                }
            }
        }

        private void ConnectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedConnectionManager))
            {
                var ui = _selectedConnectionManager;

                var cfg = ui.LoadUserSettings();
                var ctrl = ui.GenerateUiInterface(cfg);

                SelectedConnectionManagerControl = (Control)ctrl;

                SelectedConnectionManagerDescription = ui.GetDescription();
            }
        }

        [ObservableProperty]
        BaseConnectManager[] _availableConnectionManagers;

        [ObservableProperty]
        BaseConnectManager _selectedConnectionManager;

        [ObservableProperty]
        Control _selectedConnectionManagerControl;

        [ObservableProperty]
        string _selectedConnectionManagerDescription;

        [ObservableProperty]
        bool _isConnected;


        [RelayCommand()]
        void OnControlLoaded()
        {

        }


        [RelayCommand()]
        void Connect()
        {
            var t = _selectedConnectionManager;
            
            var ctrl = _selectedConnectionManagerControl as BaseDaqConfigGUIControl;

            if (ctrl == null)
                return;

            if (!ctrl.IsValidConfig())
            {
                //ShoeMessageToUser("Invalid/incomplete config");
                return;
            }


            {
                var ui = _selectedConnectionManager;// Context.SelectedDevice.UI;

                var config = ctrl.GetUserSettings();

                ui.SaveUserSettings(config);
                
                var calib = ui.LoadCalibrationSettings();
                
                var daqInterface = _selectedConnectionManager.GenerateDaqInterface(calib, config);

                {
                    var sr = config.GetAdcSampleRate();
                    var rd = config.GetRecordDepth();
                    var chnCount = daqInterface.GetMaxChannelCount();

                    LibRuntimeVariables.Instance.CurrentRepo.Init(sr, rd, chnCount);
                }
                
                daqInterface.TargetRepository = LibRuntimeVariables.Instance.CurrentRepo;
                LibRuntimeVariables.Instance.CurrentDaq = daqInterface;
                UiRuntimeVariables.Instance.StartDaqThread(daqInterface);
            }


            this.IsConnected = true;
        }

        [RelayCommand()]
        void DisConnect()
        {
            UiRuntimeVariables.Instance.StopDaqThread();
            //LibRuntimeVariables.Instance.CurrentDaq.StopAdc();
            LibRuntimeVariables.Instance.CurrentDaq.DisConnect();
            this.IsConnected= false;
        }
    }
}
