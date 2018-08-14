using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SettingsView.Resources
{
    public class ConnectionBackgroundCoverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            if ((bool)value)
            {
                return Brushes.White;
            }
            return Brushes.Gray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            if (!(bool)value)
            {
                return Brushes.Gray;
            }
            return Brushes.White;
        }
        #endregion
    }
}
