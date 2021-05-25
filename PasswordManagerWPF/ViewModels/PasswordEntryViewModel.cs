using PasswordManagerCore.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PasswordManagerWPF
{
    public class PasswordEntryViewModel : BaseViewModel
    {
        #region Constructor

        public PasswordEntryViewModel()
        {
            Passwords.Add(new PasswordEntryModel()
            {
                Title = "Example site",
                Username = "Example username",
                Url = "example.com",
                Note = "Dd"
            });
        }

        #endregion

        #region Public properties

        public ObservableCollection<PasswordEntryModel> Passwords { get; set; } = new ObservableCollection<PasswordEntryModel>();

        public PasswordEntryModel SelectedEntry
        {
            get => _SelectedEntry;
            set
            {
                if (_SelectedEntry == value)
                    return;
                _SelectedEntry = value;
                //PasswordControlVisible = true;
                OnPropertyChanged();
            }
        }

        public bool PasswordControlVisible
        {
            get => _PasswordControlVisible;
            set
            {
                if (_PasswordControlVisible == value)
                    return;
                _PasswordControlVisible = value;
                OnPropertyChanged();
            }
        }

        public int PasswordLenght
        {
            get => _PasswordLenght;
            set
            {
                if (_PasswordLenght == value)
                    return;
                _PasswordLenght = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private properties

        private PasswordEntryModel _SelectedEntry = new PasswordEntryModel()
        {
            Title = "Title",
            Username = "Username",
            Url = "URL",
            Note = "Note"
        };
        private bool _PasswordControlVisible = false;
        private int _PasswordLenght = 6;

        #endregion

        #region Commands

        public ICommand OpenDbCommand => new RelayCommand(OpenDb);
        public ICommand CloseDbCommand => new RelayCommand(CloseDb);
        public ICommand PwdEntryNewCommand => new RelayCommand(PwdEntryNew);
        public ICommand PwdEntryDeleteCommand => new RelayCommand(PwdEntryDelete);
        public ICommand HideControlCommand => new RelayCommand(HideControl);

        #endregion

        private void OpenDb()
        {
            throw new NotImplementedException();
        }

        private void CloseDb()
        {
            throw new NotImplementedException();
        }

        int i = 0;
        private void PwdEntryNew()
        {
            var newEntry = new PasswordEntryModel() { Username = "Person " + i.ToString() };
            i++;
            Passwords.Add(newEntry);
            SelectedEntry = newEntry;
            PasswordControlVisible = true;
        }

        private void PwdEntryDelete()
        {
            Passwords.Remove(SelectedEntry);
            // if there is no more entires hide password edit control
            if (Passwords.Count == 0) HideControl();
        }


        private void HideControl()
        {
            PasswordControlVisible = false;
        }

        private void ToggleControl()
        {
            PasswordControlVisible ^= true;
        }
    }
}
