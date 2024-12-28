using Avalonia.Media;
using Persiscope.Ui;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ChannelSetting> _channels;



        public SettingsViewModel()
        {
            var chs = this.Channels = new ObservableCollection<ChannelSetting>();

            foreach (var channel in UiRuntimeVariables.Instance.Channels)
            {
                var c = new ChannelSetting();

                c.HaltPrpChanged = true;
                c.Parent = this;
                c.Enabled = channel.Enabled;
                c.Id = channel.Id;
                c.Color = channel.DisplayColor;
                c.HaltPrpChanged = false;

                chs.Add(c);
            }
        }

        public void Save(ChannelSetting chn)
        {
            var ch = UiRuntimeVariables.Instance.Channels.FirstOrDefault(i => i.Id == chn.Id);

            if(ch != null)
            {
                ch.DisplayColor = chn.Color;
                ch.Enabled = chn.Enabled;
            }
        }

        public partial class ChannelSetting : ObservableObject
        {
            public SettingsViewModel Parent;

            public bool HaltPrpChanged;

            [ObservableProperty]
            private string _id;

            [ObservableProperty]
            private bool _enabled;

            [ObservableProperty]
            private Color _color;


            public ChannelSetting()
            {
                this.PropertyChanged += onPropertyChanged;
            }

            private void onPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (!HaltPrpChanged)
                    Parent.Save(this);
            }
        }
    }
}
