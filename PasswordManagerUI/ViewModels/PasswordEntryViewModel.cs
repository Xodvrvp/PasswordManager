using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerUI
{
    public class PasswordEntryViewModel : BaseViewModel
    {
        #region Constructor

        public PasswordEntryViewModel(string title, string username, SecureString password, string url, string note)
        {
            this.Title = title;
            this.UserName = username;
            this.Password = password;
            this.Url = url;
            this.Note = note;
        }

        #endregion

        #region Public properties

        public string Title
        { 
            get => _Title;
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName == value)
                    return;
                _UserName = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get => _Password;
            set
            {
                if (_Password == value)
                    return;
                _Password = value;
                OnPropertyChanged();
            }
        }

        public string Url
        {
            get => _Url;
            set
            {
                if (_Url == value)
                    return;
                _Url = value;
                OnPropertyChanged();
            }
        }

        public string Note
        {
            get => _Note;
            set
            {
                if (_Note == value)
                    return;
                _Note = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private properties

        private string _Title;
        private string _UserName;
        private SecureString _Password;
        private string _Url;
        private string _Note;

        #endregion

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
