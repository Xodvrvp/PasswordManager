using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerWPF
{
    public enum CurrentControl
    {
        PasswordEntry,
    }

    public class CurrentControlConverter : BaseConverter<CurrentControlConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //switch (CurrentControl)
            //{
            //    case CurrentControl.PasswordEntry:
            //        break;
            //    default:
            //        break;
            //}
            return "none";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
