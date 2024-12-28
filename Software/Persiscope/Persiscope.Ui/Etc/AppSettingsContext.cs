using AvlScope.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvlScope.Ui.Etc
{
    public class AppSettingsChannelInfo : INotifyPropertyChanged
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

        #region Id Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (AreEqualObjects(_Id, value))
                    return;

                var _fieldOldValue = _Id;

                _Id = value;

                AppSettingsChannelInfo.OnIdChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("Id");
            }
        }

        private int _Id;

        public EventHandler<PropertyValueChangedEventArgs<int>> IdChanged;

        public static void OnIdChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AppSettingsChannelInfo;

            if (obj.IdChanged != null)
                obj.IdChanged(obj, e);
        }

        #endregion

        #region Enabled Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (AreEqualObjects(_Enabled, value))
                    return;

                var _fieldOldValue = _Enabled;

                _Enabled = value;

                AppSettingsChannelInfo.OnEnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                this.OnPropertyChanged("Enabled");
            }
        }

        private bool _Enabled;

        public EventHandler<PropertyValueChangedEventArgs<bool>> EnabledChanged;

        public static void OnEnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
        {
            var obj = sender as AppSettingsChannelInfo;

            if (obj.EnabledChanged != null)
                obj.EnabledChanged(obj, e);
        }

        #endregion

        #region HardwareChannel Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public int HardwareChannel
        {
            get { return _HardwareChannel; }
            set
            {
                if (AreEqualObjects(_HardwareChannel, value))
                    return;

                var _fieldOldValue = _HardwareChannel;

                _HardwareChannel = value;

                AppSettingsChannelInfo.OnHardwareChannelChanged(this, new PropertyValueChangedEventArgs<int>(_fieldOldValue, value));

                this.OnPropertyChanged("HardwareChannel");
            }
        }

        private int _HardwareChannel;

        public EventHandler<PropertyValueChangedEventArgs<int>> HardwareChannelChanged;

        public static void OnHardwareChannelChanged(object sender, PropertyValueChangedEventArgs<int> e)
        {
            var obj = sender as AppSettingsChannelInfo;

            if (obj.HardwareChannelChanged != null)
                obj.HardwareChannelChanged(obj, e);
        }

        #endregion


    }

    public class AppSettingsContext : INotifyPropertyChanged
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

		#region Channels Property and field

		[Obfuscation(Exclude = true, ApplyToMembers = false)]
		public ObservableCollection<AppSettingsChannelInfo> Channels
		{
			get { return _Channels; }
			set
			{
				if (AreEqualObjects(_Channels, value))
					return;

				var _fieldOldValue = _Channels;

				_Channels = value;

				AppSettingsContext.OnChannelsChanged(this, new PropertyValueChangedEventArgs<ObservableCollection<AppSettingsChannelInfo>>(_fieldOldValue, value));

				this.OnPropertyChanged("Channels");
			}
		}

		private ObservableCollection<AppSettingsChannelInfo> _Channels;

		public EventHandler<PropertyValueChangedEventArgs<ObservableCollection<AppSettingsChannelInfo>>> ChannelsChanged;

		public static void OnChannelsChanged(object sender, PropertyValueChangedEventArgs<ObservableCollection<AppSettingsChannelInfo>> e)
		{
			var obj = sender as AppSettingsContext;

			if (obj.ChannelsChanged != null)
				obj.ChannelsChanged(obj, e);
		}

		#endregion



		

		


    }
}
