using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManagerWPF
{
    /// <summary>
    /// Interaction logic for PasswordsPage.xaml
    /// </summary>
    public partial class PasswordsPage : Page
    {
        private DataGridCell _currentCell;
        public PasswordsPage()
        {
            InitializeComponent();
            //this.DataContext = new PasswordEntryViewModel();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is DataGridCell)
                _currentCell = (DataGridCell)e.OriginalSource;
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}