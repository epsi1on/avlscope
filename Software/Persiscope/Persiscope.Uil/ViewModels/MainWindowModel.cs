using Avalonia.Controls;
using Persiscope.Common;
using Persiscope.Ui;
using Persiscope.Uil.Models;
using Persiscope.Uil.ViewModels.Plot;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.ViewModels
{
    public partial class MainWindowModel : ViewModelBase
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

        public MainWindowModel()
        {
            Items = new ObservableCollection<MainMenuItem>(GetMainMenueItems());

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(ConnectViewModel));

            UiRuntimeVariables.Instance.IsConnectedChanged += Instance_IsConnectedChanged;

            Instance_IsConnectedChanged(null, false);


            var tit = UiRuntimeVariables.AppName;

            if (!string.IsNullOrEmpty(UiRuntimeVariables.VersionPrefix))
                tit += " - " + UiRuntimeVariables.VersionPrefix;

            this.WindowTitle = tit;


        }


        [ObservableProperty]
        public ObservableCollection<MainMenuItem> _items;


        [ObservableProperty]
        private bool _isPaneOpen;

		
		[ObservableProperty]
		private ViewModelBase _currentPage = new ConnectViewModel();


        [ObservableProperty]
        private MainMenuItem _selectedListItem;


        [ObservableProperty]
        private bool _isConnected;

        [ObservableProperty]
        private string _windowTitle;



        private static MainMenuItem[] GetMainMenueItems()
		{
			var buf = new List<MainMenuItem>();

            {
                var item = new MainMenuItem(typeof(SettingsViewModel), "Gear", "Settings", "Static Settings of app");

                item.AlwaysEnabled = true;
                item.Enabled = true;

                buf.Add(item);
            }

            {
                var item = new MainMenuItem(typeof(ConnectViewModel), "Connect", "Connect", "Connect to device (hardware)");

                item.AlwaysEnabled = true;
                item.Enabled = true;

                buf.Add(item);
            }

            {
                var item = new MainMenuItem(typeof(EkgViewModel), "Ekg", "EKG", "Electro Kardio Gram");

                item.AlwaysEnabled = false;
                item.Enabled = true;

                buf.Add(item);
            }

            {
                var item = new MainMenuItem(typeof(HarmonicViewModel), "SinWave", "Harmonic (Auto)", "Smart Harmonic Signal Visualizer");

                item.AlwaysEnabled = false;
                item.Enabled = true;

                buf.Add(item);
            }

            {
                var item = new MainMenuItem(typeof(FftViewModel), "FFT", "FFT", "FFT Visualizer");

                item.AlwaysEnabled = false;
                item.Enabled = true;

                buf.Add(item);
            }

            {
                var item = new MainMenuItem(typeof(AboutViewModel), "Info", "About", "About this software");

                item.AlwaysEnabled = true;
                item.Enabled = true;

                buf.Add(item);
            }


            return buf.ToArray();
		}

        partial void OnSelectedListItemChanged(MainMenuItem value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ViewModelBase vmb) return;

            CurrentPage = vmb;
        }


        public void Init()
		{

		}

        private void Instance_IsConnectedChanged(object? sender, bool e)
        {
            //if (_isConnected == e)
            //    return;

            IsConnected = e;

            foreach (var item in Items)
            {
                if (!item.AlwaysEnabled)
                    item.Enabled = IsConnected;
            }

            if (!IsConnected)

                SelectedListItem = Items.First(vm => vm.ModelType == typeof(ConnectViewModel));
        }

        ~MainWindowModel()
        {
            UiRuntimeVariables.Instance.IsConnectedChanged -= Instance_IsConnectedChanged;
        }

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}
