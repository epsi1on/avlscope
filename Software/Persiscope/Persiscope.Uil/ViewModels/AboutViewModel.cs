using Persiscope.Ui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.ViewModels
{
    public partial class AboutViewModel : ViewModelBase
    {

        public AboutViewModel() 
        {
            var asm = Assembly.GetExecutingAssembly();
            this.AppName = UiRuntimeVariables.AppName;


            if (!string.IsNullOrEmpty(UiRuntimeVariables.VersionPrefix))
            {
                this.VerPrefix += UiRuntimeVariables.VersionPrefix;
            }

            this.Ver = asm.GetName().Version.ToString();
            this.Description = UiRuntimeVariables.AppDescription;

            this.Dependencies = new ObservableCollection<DependencyInfo>();

            this.Dependencies.Add(new DependencyInfo("Avalonia UI", "https://avaloniaui.net/", "for cross platform GUI"));
            this.Dependencies.Add(new DependencyInfo("FFTW", "https://www.fftw.org/", "for calculating FFT"));
            this.Dependencies.Add(new DependencyInfo("Sharp FFTW", "https://github.com/wo80/SharpFFTW", "for comunicating with FFTW"));
            
        }

        [ObservableProperty]
        string _ver;

        [ObservableProperty]
        string _appName;

        [ObservableProperty]
        string _description;

        [ObservableProperty]
        string _verPrefix;

        [ObservableProperty]
        ObservableCollection<DependencyInfo> _dependencies;


        public partial class DependencyInfo : ObservableObject
        {

            public DependencyInfo()
            {

            }

            public DependencyInfo(string title,string url,string shortDesc)
            {
                this._title = title;
                this._url = url;
                this._shortDesc = shortDesc;
            }


            [ObservableProperty]
            string _title;

            [ObservableProperty]
            string _url;

            [ObservableProperty]
            string _shortDesc;

            [RelayCommand]
            public void UrlLink(object url)
            {
                var inf = url as DependencyInfo;

                Process.Start(new ProcessStartInfo() { FileName = inf.Url, UseShellExecute = true });
            }

        }
    }
}

