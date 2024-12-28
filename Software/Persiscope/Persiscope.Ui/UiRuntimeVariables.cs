using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvlScope.Ui
{
    public class UiRuntimeVariables
    {
        public static readonly UiRuntimeVariables Instance;

        public UiRuntimeVariables() { }

        public ChannelInfo[] Channels;


        static UiRuntimeVariables()
        {
            Instance = new UiRuntimeVariables();

            Instance.Channels = new ChannelInfo[3];

            Instance.Channels[0] = new ChannelInfo() { DisplayColor = Colors.Red, Enabled = true, Id = "Ch1", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 0 } };

            Instance.Channels[1] = new ChannelInfo() { DisplayColor = Colors.Blue, Enabled = true, Id = "Ch2", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 1 } };

            Instance.Channels[2] = new ChannelInfo() { DisplayColor = Colors.Cyan, Enabled = true, Id = "Ch3", Source = new DirectHardwareChannelSource() { TargetChannelIndex = 2 } };
        }

    }

   

}
