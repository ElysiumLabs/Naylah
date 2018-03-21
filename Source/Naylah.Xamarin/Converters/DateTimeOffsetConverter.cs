using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Converters
{
    public class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset? teste = value as DateTimeOffset?;
            if (teste.HasValue)
            {
                if (parameter != null)
                {
                    return teste.Value.ToString(parameter as string).ToUpper();
                }
                return teste.Value.ToString("dd/MM/yyyy HH:mm");
            }

            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
