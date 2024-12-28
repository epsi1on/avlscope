using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Persiscope.Hardware;
using Persiscope.Hardware.Pico.Picov1;

namespace Persiscope.UiHardware.Rp2040Raw;

public partial class Connect : UserControl, BaseDaqConfigGUIControl
{
    public Connect()
    {
        InitializeComponent();

        this.DataContext = this.Context = new ConnectContext();
    }

    ConnectContext Context;

    
    public void Init()
    {
        Context.RefreshPorts();
    }

    public bool IsValidConfig()
    {
        string msg;

        {
            var cnt = 0;

            if (Context.Gpio26Enabled) cnt++;
            if (Context.Gpio27Enabled) cnt++;
            if (Context.Gpio28Enabled) cnt++;
            if (Context.InternalReferenceEnabled) cnt++;
            if (Context.InternalTempSensorEnabled) cnt++;

            if (cnt == 0) return false;
        }

        {
            if (Context.SampleRate <= 0) return false;

            if (Context.SampleRate > 500_000) return false;

            if (Context.SelectedPort == null) return false;

            var availableBitWidths = new int[] { 12 };

            if (!availableBitWidths.Contains(Context.BitWidth)) return false;

        }

        return true;

        throw new NotImplementedException();
    }

    public void SetDefaultUserSettings(BaseDeviceUserSettingsData config)
    {
        Context.SetCurrentUserSettings(config as Rp2daqMchUserSettings);
    }

    private void BtnRefreshPorts_Click(object sender, RoutedEventArgs e)
    {
        Context.RefreshPorts();
    }

    public BaseDeviceUserSettingsData GetUserSettings()
    {
        return Context.GetCurrentUserSettings();
    }
}