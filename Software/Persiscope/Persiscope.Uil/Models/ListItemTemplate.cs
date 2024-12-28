using Persiscope.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.Models
{
    //public record ListItemTemplate(Type ModelType, string IconKey, string Label, bool Enabled = true);

    public class MainMenuItem : INotifyPropertyChanged
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


        public MainMenuItem(Type modelType, string iconKey, string label, string description)
        {
            _ModelType = modelType;
            _IconKey = iconKey;
            _Label = label;
            _Description = description;
        }

        #region ModelType Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public Type ModelType
        {
            get { return _ModelType; }
            set
            {
                if (AreEqualObjects(_ModelType, value))
                    return;

                var _fieldOldValue = _ModelType;

                _ModelType = value;

                MainMenuItem.OnModelTypeChanged(this, new PropertyValueChangedEventArgs<Type>(_fieldOldValue, value));

                this.OnPropertyChanged("ModelType");
            }
        }

        private Type _ModelType;

        public EventHandler<PropertyValueChangedEventArgs<Type>> ModelTypeChanged;

        public static void OnModelTypeChanged(object sender, PropertyValueChangedEventArgs<Type> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.ModelTypeChanged != null)
                obj.ModelTypeChanged(obj, e);
        }

        #endregion





        #region IconKey Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string IconKey
        {
            get { return _IconKey; }
            set
            {
                if (AreEqualObjects(_IconKey, value))
                    return;

                var _fieldOldValue = _IconKey;

                _IconKey = value;

                MainMenuItem.OnIconKeyChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("IconKey");
            }
        }

        private string _IconKey;

        public EventHandler<PropertyValueChangedEventArgs<string>> IconKeyChanged;

        public static void OnIconKeyChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.IconKeyChanged != null)
                obj.IconKeyChanged(obj, e);
        }

        #endregion

        #region Label Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string Label
        {
            get { return _Label; }
            set
            {
                if (AreEqualObjects(_Label, value))
                    return;

                var _fieldOldValue = _Label;

                _Label = value;

                MainMenuItem.OnLabelChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("Label");
            }
        }

        private string _Label;

        public EventHandler<PropertyValueChangedEventArgs<string>> LabelChanged;

        public static void OnLabelChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.LabelChanged != null)
                obj.LabelChanged(obj, e);
        }

        #endregion

        #region Description Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public string Description
        {
            get { return _Description; }
            set
            {
                if (AreEqualObjects(_Description, value))
                    return;

                var _fieldOldValue = _Description;

                _Description = value;

                MainMenuItem.OnDescriptionChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

                this.OnPropertyChanged("Description");
            }
        }

        private string _Description;

        public EventHandler<PropertyValueChangedEventArgs<string>> DescriptionChanged;

        public static void OnDescriptionChanged(object sender, PropertyValueChangedEventArgs<string> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.DescriptionChanged != null)
                obj.DescriptionChanged(obj, e);
        }

        #endregion

        #region AlwaysEnabled Property and field

        [Obfuscation(Exclude = true, ApplyToMembers = false)]
        public bool AlwaysEnabled
        {
            get { return _AlwaysEnabled; }
            set
            {
                if (AreEqualObjects(_AlwaysEnabled, value))
                    return;

                var _fieldOldValue = _AlwaysEnabled;

                _AlwaysEnabled = value;

                MainMenuItem.OnAlwaysEnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                this.OnPropertyChanged("AlwaysEnabled");
            }
        }

        private bool _AlwaysEnabled;

        public EventHandler<PropertyValueChangedEventArgs<bool>> AlwaysEnabledChanged;

        public static void OnAlwaysEnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.AlwaysEnabledChanged != null)
                obj.AlwaysEnabledChanged(obj, e);
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

                MainMenuItem.OnEnabledChanged(this, new PropertyValueChangedEventArgs<bool>(_fieldOldValue, value));

                this.OnPropertyChanged("Enabled");
            }
        }

        private bool _Enabled;

        public EventHandler<PropertyValueChangedEventArgs<bool>> EnabledChanged;

        public static void OnEnabledChanged(object sender, PropertyValueChangedEventArgs<bool> e)
        {
            var obj = sender as MainMenuItem;

            if (obj.EnabledChanged != null)
                obj.EnabledChanged(obj, e);
        }

        #endregion


    }
}
