using Communication.Infrastructure;
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
    public class TypeBackgroundConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            MessageTypeEnum val = (MessageTypeEnum)value;
            if (val == MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            }
            else if (val == MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            }
            else if (val == MessageTypeEnum.INFO)
            {
                return Brushes.LightGreen;
            }
            return Brushes.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            return value;
        }
        #endregion
    }
}
