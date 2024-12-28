using Persiscope.Common;
using Persiscope.Hardware.Pico;
using Persiscope.Hardware.Pico.Picov1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContextClass = Persiscope.UiHardware.Rp2040Raw.ConnectContext;


namespace Persiscope.UiHardware.Rp2040Raw;

public class ConnectContext : INotifyPropertyChanged
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

        if (PropertyChanged != null)
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion


    #region RecordDepthSecs Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public double RecordDepthSecs
    {
        get { return _RecordDepthSecs; }
        set
        {
            if (AreEqualObjects(_RecordDepthSecs, value))
                return;

            var _fieldOldValue = _RecordDepthSecs;

            _RecordDepthSecs = value;

            ConnectContext.OnRecordDepthSecsChanged(this, new PropertyValueChangedEventArgs<double>(_fieldOldValue, value));

            this.OnPropertyChanged("RecordDepthSecs");
        }
    }

    private double _RecordDepthSecs;

    public EventHandler<PropertyValueChangedEventArgs<double>> RecordDepthSecsChanged;

    public static void OnRecordDepthSecsChanged(object sender, PropertyValueChangedEventArgs<double> e)
    {
        var obj = sender as ConnectContext;

        if (obj.RecordDepthSecsChanged != null)
            obj.RecordDepthSecsChanged(obj, e);
    }

    #endregion

    #region SampleRate Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public long SampleRate
    {
        get { 
            return _SampleRate;
        }
        set
        {
            if (AreEqualObjects(_SampleRate, value))
                return;

            var _fieldOldValue = _SampleRate;

            _SampleRate = value;

            ContextClass.OnSampleRateChanged(this, new PropertyValueChangedEventArgs<long>(_fieldOldValue, value));

            this.OnPropertyChanged("SampleRate");
        }
    }

    private long _SampleRate;

    public EventHandler<PropertyValueChangedEventArgs<long>> SampleRateChanged;

    public static void OnSampleRateChanged(object sender, PropertyValueChangedEventArgs<long> e)
    {
        var obj = sender as ContextClass;

        if (obj.SampleRateChanged != null)
            obj.SampleRateChanged(obj, e);
    }

    #endregion

    #region AvailablePorts Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public ObservableCollection<string> AvailablePorts
    {
        get { return _AvailablePorts; }
        set
        {
            if (AreEqualObjects(_AvailablePorts, value))
                return;

            var _fieldOldValue = _AvailablePorts;

            _AvailablePorts = value;

            ContextClass.OnAvailablePortsChanged(this, new PropertyValueChangedEventArgs<ObservableCollection<string>>(_fieldOldValue, value));

            this.OnPropertyChanged("AvailablePorts");
        }
    }

    private ObservableCollection<string> _AvailablePorts;

    public EventHandler<PropertyValueChangedEventArgs<ObservableCollection<string>>> AvailablePortsChanged;

    public static void OnAvailablePortsChanged(object sender, PropertyValueChangedEventArgs<ObservableCollection<string>> e)
    {
        var obj = sender as ContextClass;

        if (obj.AvailablePortsChanged != null)
            obj.AvailablePortsChanged(obj, e);
    }

    #endregion

    #region SelectedPort Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public string SelectedPort
    {
        get { return _SelectedPort; }
        set
        {
            if (AreEqualObjects(_SelectedPort, value))
                return;

            var _fieldOldValue = _SelectedPort;

            _SelectedPort = value;

            ContextClass.OnSelectedPortChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

            this.OnPropertyChanged("SelectedPort");
        }
    }

    private string _SelectedPort;

    public EventHandler<PropertyValueChangedEventArgs<string>> SelectedPortChanged;

    public static void OnSelectedPortChanged(object sender, PropertyValueChangedEventArgs<string> e)
    {
        var obj = sender as ContextClass;

        if (obj.SelectedPortChanged != null)
            obj.SelectedPortChanged(obj, e);
    }

    #endregion

    #region BitWidth Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public int BitWidth
    {
        get { 
            return _BitWidth; 
        }
        set
        {
            if (AreEqualObjects(_BitWidth, value))
                return;

            var _fieldOldValue = _BitWidth;

            _BitWidth = value;

            ContextClass.OnBitWidthChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

            this.OnPropertyChanged("BitWidth");
        }
    }

    private int _BitWidth;

    public EventHandler<PropertyValueChangedEventArgs<int>> BitWidthChanged;

    public static void OnBitWidthChanged(object sender, PropertyValueChangedEventArgs<int> e)
    {
        var obj = sender as ContextClass;

        if (obj.BitWidthChanged != null)
            obj.BitWidthChanged(obj, e);
    }

    #endregion

    #region Gpio26Enabled Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public bool Gpio26Enabled
    {
        get { return _Gpio26Enabled; }
        set
        {
            if (AreEqualObjects(_Gpio26Enabled, value))
                return;

            var _fieldOldValue = _Gpio26Enabled;

            _Gpio26Enabled = value;

            ContextClass.OnGpio26EnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

            this.OnPropertyChanged("Gpio26Enabled");
        }
    }

    private bool _Gpio26Enabled;

    public EventHandler<PropertyValueChangedEventArgs<bool>> Gpio26EnabledChanged;

    public static void OnGpio26EnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
    {
        var obj = sender as ContextClass;

        if (obj.Gpio26EnabledChanged != null)
            obj.Gpio26EnabledChanged(obj, e);
    }

    #endregion

    #region Gpio27Enabled Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public bool Gpio27Enabled
    {
        get { return _Gpio27Enabled; }
        set
        {
            if (AreEqualObjects(_Gpio27Enabled, value))
                return;

            var _fieldOldValue = _Gpio27Enabled;

            _Gpio27Enabled = value;

            ContextClass.OnGpio27EnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

            this.OnPropertyChanged("Gpio27Enabled");
        }
    }

    private bool _Gpio27Enabled;

    public EventHandler<PropertyValueChangedEventArgs<bool>> Gpio27EnabledChanged;

    public static void OnGpio27EnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
    {
        var obj = sender as ContextClass;

        if (obj.Gpio27EnabledChanged != null)
            obj.Gpio27EnabledChanged(obj, e);
    }

    #endregion

    #region Gpio28Enabled Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public bool Gpio28Enabled
    {
        get { return _Gpio28Enabled; }
        set
        {
            if (AreEqualObjects(_Gpio28Enabled, value))
                return;

            var _fieldOldValue = _Gpio28Enabled;

            _Gpio28Enabled = value;

            ContextClass.OnGpio28EnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

            this.OnPropertyChanged("Gpio28Enabled");
        }
    }

    private bool _Gpio28Enabled;

    public EventHandler<PropertyValueChangedEventArgs<bool>> Gpio28EnabledChanged;

    public static void OnGpio28EnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
    {
        var obj = sender as ContextClass;

        if (obj.Gpio28EnabledChanged != null)
            obj.Gpio28EnabledChanged(obj, e);
    }

    #endregion

    #region InternalReferenceEnabled Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public bool InternalReferenceEnabled
    {
        get { return _InternalReferenceEnabled; }
        set
        {
            if (AreEqualObjects(_InternalReferenceEnabled, value))
                return;

            var _fieldOldValue = _InternalReferenceEnabled;

            _InternalReferenceEnabled = value;

            ContextClass.OnInternalReferenceEnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

            this.OnPropertyChanged("InternalReferenceEnabled");
        }
    }

    private bool _InternalReferenceEnabled;

    public EventHandler<PropertyValueChangedEventArgs<bool>> InternalReferenceEnabledChanged;

    public static void OnInternalReferenceEnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
    {
        var obj = sender as ContextClass;

        if (obj.InternalReferenceEnabledChanged != null)
            obj.InternalReferenceEnabledChanged(obj, e);
    }

    #endregion

    #region InternalTempSensorEnabled Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public bool InternalTempSensorEnabled
    {
        get { return _InternalTempSensorEnabled; }
        set
        {
            if (AreEqualObjects(_InternalTempSensorEnabled, value))
                return;

            var _fieldOldValue = _InternalTempSensorEnabled;

            _InternalTempSensorEnabled = value;

            ContextClass.OnInternalTempSensorEnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

            this.OnPropertyChanged("InternalTempSensorEnabled");
        }
    }

    private bool _InternalTempSensorEnabled;

    public EventHandler<PropertyValueChangedEventArgs<bool>> InternalTempSensorEnabledChanged;

    public static void OnInternalTempSensorEnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
    {
        var obj = sender as ContextClass;

        if (obj.InternalTempSensorEnabledChanged != null)
            obj.InternalTempSensorEnabledChanged(obj, e);
    }

    #endregion





    internal void RefreshPorts()
    {
        this.AvailablePorts = new ObservableCollection<string>(SerialPort.GetPortNames());
        this.SelectedPort = this.AvailablePorts.FirstOrDefault();
    }

    public Rp2daqMchUserSettings GetCurrentUserSettings()
    {
        var cfg = new Rp2daqMchUserSettings();

        cfg.SampleRate = (int)this.SampleRate;
        cfg.ComPortName = this.SelectedPort;
        //cfg.ChannelId = this.SelectedChannel.ChannelId;

        var lst = new List<Rp2040AdcChannels>();

        if (Gpio26Enabled)
            lst.Add(Rp2040AdcChannels.Gpio26);

        if (Gpio27Enabled)
            lst.Add(Rp2040AdcChannels.Gpio27);

        if (Gpio28Enabled)
            lst.Add(Rp2040AdcChannels.Gpio28);

        if (InternalReferenceEnabled)
            lst.Add(Rp2040AdcChannels.InternalReference);

        if (InternalTempSensorEnabled)
            lst.Add(Rp2040AdcChannels.InternalTempratureSensor);


        cfg.ChannelIds = lst.ToArray();
        cfg.BitWidth = this.BitWidth;
        cfg.RecordDepthSecs = this.RecordDepthSecs;


        return (cfg);
    }

    public void SetCurrentUserSettings(Rp2daqMchUserSettings data)
    {
        var cfg = (data);
        this.SampleRate = cfg.SampleRate;
        this.SelectedPort = cfg.ComPortName;

        if (data.ChannelIds != null)
        {
            this.Gpio26Enabled = data.ChannelIds.Contains(Rp2040AdcChannels.Gpio26);
            this.Gpio27Enabled = data.ChannelIds.Contains(Rp2040AdcChannels.Gpio27);
            this.Gpio28Enabled = data.ChannelIds.Contains(Rp2040AdcChannels.Gpio28);
            this.InternalReferenceEnabled = data.ChannelIds.Contains(Rp2040AdcChannels.InternalReference);
            this.InternalTempSensorEnabled = data.ChannelIds.Contains(Rp2040AdcChannels.InternalTempratureSensor);
        }

        this.RecordDepthSecs = data.RecordDepthSecs;

        this.BitWidth = cfg.BitWidth;
    }

}
