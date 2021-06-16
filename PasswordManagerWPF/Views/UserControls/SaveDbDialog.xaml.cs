using System.Windows;
using System.Windows.Controls;

namespace PasswordManagerWPF
{
    /// <summary>
    /// Interaction logic for SaveDbDialog.xaml
    /// </summary>
    public partial class SaveDbDialog : UserControl
    {
        public SaveDbDialog()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).DbPassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
