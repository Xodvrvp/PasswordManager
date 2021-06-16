using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerWPF
{
    public class FileDialogViewModel : BaseViewModel
    {
        public FileDialogViewModel()
        {

        }

        public string FilePath
        {
            get => _FilePath;
            set
            {
                if (_FilePath == value)
                    return;
                _FilePath = value;
                OnPropertyChanged();
            }
        }

        private string _FilePath;
    }
}
