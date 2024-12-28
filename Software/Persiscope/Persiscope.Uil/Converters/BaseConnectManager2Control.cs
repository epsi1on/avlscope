using Avalonia.Data.Converters;
using Persiscope.Hardware;
using Persiscope.Uil.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil.Converters
{
    public class BaseConnectManager2Control : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var ui = value as BaseConnectManager;
            
            if (ui == null)
                return null;

            var cfg = ui.LoadUserSettings();
            var ctrl = ui.GenerateUiInterface(cfg);

            return ctrl;
            throw new NotImplementedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
