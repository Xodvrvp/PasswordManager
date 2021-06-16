using System;
using System.Globalization;
using System.Text;

namespace PasswordManagerWPF
{
    public class StringToStars : BaseConverter<StringToStars>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var str = (String)value;
            StringBuilder stars = new StringBuilder();
            for (int i = 0; i < str.Length; i++) stars.Append("*");
            return stars.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
