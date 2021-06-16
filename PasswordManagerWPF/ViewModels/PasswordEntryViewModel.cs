using MaterialDesignThemes.Wpf;
using PasswordManagerCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PasswordManagerWPF
{
    public class PasswordEntryViewModel : BaseViewModel
    {
        #region Constructor

        public PasswordEntryViewModel()
        {
        }

        #endregion

        #region Public properties

        public ObservableCollection<PasswordEntryModel> Passwords { get; set; } = new ObservableCollection<PasswordEntryModel>();
        public PasswordEntryModel SelectedEntry
        {
            get => _SelectedEntry;
            set
            {
                if (_SelectedEntry == value) return;
                _SelectedEntry = value;
                //PasswordControlVisible = true;
                OnPropertyChanged();
            }
        }

        public SecureString DbPassword 
        { 
            get => _DbPassword; 
            set
            {
                if (_DbPassword == value) return;
                _DbPassword = value;
                //SelectedEntry.SecurePassword = _DbPassword;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllowAccept));
                //OnPropertyChanged(nameof(SelectedEntry));
            }
        }        
        public string DatabasePath
        {
            get => _DatabasePath;
            set
            {
                if (_DatabasePath == value) return;
                _DatabasePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllowAccept));
            }
        }

        public bool AllowAccept
        {
            get => IsDbSelectedAndPasswordFilled();
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
        public bool SelectionMode
        {
            get => _SelectionMode;
            set
            {
                if (_SelectionMode == value) return;
                _SelectionMode = value;
                OnPropertyChanged();
            }
        }
        public bool PasswordVisible
        {
            get => _PasswordVisibile;
            set
            {
                if (_PasswordVisibile == value) return;
                _PasswordVisibile = value;
                OnPropertyChanged();
            }
        }

        public int PasswordLenght
        {
            get => _PasswordLenght;
            set
            {
                if (_PasswordLenght == value || value > 30 || value < 6) return;
                _PasswordLenght = value;
                OnPropertyChanged();
                GeneratePassword();
            }
        }
        public bool PasswordUpperCaseLetters
        {
            get => _PasswordUpperCaseLetters;
            set
            {
                if (_PasswordUpperCaseLetters == value) return;
                _PasswordUpperCaseLetters = value;
                OnPropertyChanged();
                GeneratePassword();
            }
        }
        public bool PasswordLowerCaseLetters
        {
            get => _PasswordLowerCaseLetters;
            set
            {
                if (_PasswordLowerCaseLetters == value) return;
                _PasswordLowerCaseLetters = value;
                OnPropertyChanged();
                GeneratePassword();
            }
        }
        public bool PasswordDigits
        {
            get => _PasswordDigits;
            set
            {
                if (_PasswordDigits == value) return;
                _PasswordDigits = value;
                OnPropertyChanged();
                GeneratePassword();
            }
        }
        public bool PasswordSpecial
        {
            get => _PasswordSpecial;
            set
            {
                if (_PasswordSpecial == value) return;
                _PasswordSpecial = value;
                OnPropertyChanged();
                GeneratePassword();
            }
        }
        public bool SliderEnabled
        {
            get => _SliderEnabled;
            set
            {
                if (_SliderEnabled == value) return;
                _SliderEnabled = value;
                OnPropertyChanged();
            }
        }

        public Action HideWindow { get; set; }
        public string ActiveWindow { get; set; }

        #endregion

        #region Private fields

        private PasswordEntryModel _SelectedEntry;
        private SecureString _DbPassword;
        private string _DatabasePath = null;
        private bool _PasswordControlVisible = false;
        private bool _SelectionMode = false;
        private bool _PasswordVisibile = false;
        private int _PasswordLenght = 15;
        private bool _PasswordUpperCaseLetters = true;
        private bool _PasswordLowerCaseLetters = true;
        private bool _PasswordDigits = true;
        private bool _PasswordSpecial = true;
        private bool _SliderEnabled = true;

        #endregion

        #region Commands

        public ICommand OpenDbCommand => new RelayCommand(OpenDb);
        public ICommand OpenAndDecryptCommand => new RelayCommand(OpenAndDecrypt);
        public ICommand SaveDbCommand => new RelayCommand(SaveDb);
        public ICommand LockDbCommand => new RelayCommand(LockDb);
        public ICommand OpenFileDialogCommand => new RelayCommand(OpenFileDialog);
        public ICommand SaveFileDialogCommand => new RelayCommand(SaveFileDialog);
        public ICommand PwdEntryNewCommand => new RelayCommand(PwdEntryNew);
        public ICommand PwdEntryDeleteCommand => new RelayCommand(PwdEntryDelete);
        public ICommand ToggleControlCommand => new RelayCommand(ToggleControl);
        public ICommand ShowPasswordCommand => new RelayCommand(ShowPasswordToggle);
        public ICommand CopyUsernameCommand => new RelayCommand(CopyUsername);
        public ICommand CopyPasswordCommand => new RelayCommand(CopyPassword);
        public ICommand CopyUrlCommand => new RelayCommand(CopyUrl);
        public ICommand AutoFillUsernameAndPasswordCommand => new RelayCommand(AutoFillUsernameAndPassword);
        public ICommand AutoFillPasswordCommand => new RelayCommand(AutoFillPassword);

        #endregion

        #region Command methods

        public async void OpenDb()
        {
            PasswordControlVisible = false;
            try
            {
                var view = new OpenDbDialog()
                {
                    DataContext = this
                };
                var result = await DialogHost.Show(view, "RootDialog");
            }
            catch (System.InvalidOperationException) { }            
        }

        private async void SaveDb()
        {
            if (DatabasePath == null || DbPassword == null)
            {
                var view = new SaveDbDialog()
                {
                    DataContext = this
                };
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandlerEncrypt);
            }
            else PasswordEntryDb.EncryptAndSerialize(DatabasePath, Passwords, DbPassword);
        }

        private async void SaveDb(string dialogHostName)
        {
            try
            {
                if (DatabasePath == null || DbPassword == null)
                {
                    var view = new SaveDbDialog()
                    {
                        DataContext = this
                    };
                    var result = await DialogHost.Show(view, dialogHostName, ClosingEventHandlerEncrypt);
                }
                else PasswordEntryDb.EncryptAndSerialize(DatabasePath, Passwords, DbPassword);
            }
            catch { }
        }

        private void ClosingEventHandlerEncrypt(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter && parameter == true)
            {
                PasswordEntryDb.EncryptAndSerialize(DatabasePath, Passwords, DbPassword);
            }
        }

        public void LockDb()
        {
            Passwords.Clear();
            DbPassword?.Dispose();
            if (PasswordControlVisible == true) ToggleControl();
        }

        private void OpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            PasswordControlVisible = false;
        }               
            
        private void OpenFileDialog()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Passwords database (*.pmdb)|*.pmdb";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) DatabasePath = openFile.FileName;
        }

        private void SaveFileDialog()
        {
            SaveFileDialog saveFile = new();
            saveFile.Filter = "Passwords database (*.pmdb)|*.pmdb";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DatabasePath = saveFile.FileName;
                SaveDb("MessageHost");
            }
        }

        private void PwdEntryNew()
        {
            var newEntry = new PasswordEntryModel();
            Passwords.Add(newEntry);
            SelectedEntry = newEntry;
            PasswordControlVisible = true;
        }

        private void PwdEntryDelete()
        {
            Passwords.Remove(SelectedEntry);
            // if there are no entires left hide password edit control
            if (Passwords.Count == 0) PasswordControlVisible = false;
        }

        private void ToggleControl()
        {
            PasswordControlVisible ^= true;
        }

        private void ShowPasswordToggle()
        {
            if (SelectedEntry != null) PasswordVisible ^= true;
        }

        private void GeneratePassword()
        {
            if (!PasswordLowerCaseLetters && !PasswordUpperCaseLetters && !PasswordDigits && !PasswordSpecial)
            {
                SliderEnabled = false;
                return;
            }
            else
            {
                SliderEnabled = true;
                SelectedEntry.Password = PasswordGenerator.GeneratePassword(PasswordLenght, PasswordLowerCaseLetters,
                        PasswordUpperCaseLetters, PasswordDigits, PasswordSpecial);                
                OnPropertyChanged(nameof(SelectedEntry));
            }            
        }

        private async void CopyUsername()
        {
            if (SelectedEntry.Username != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Username);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            }
        }

        private async void CopyPassword()
        {       
            if (SelectedEntry.Password != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Password);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            }
        }

        private async void CopyUrl()
        {
            if (SelectedEntry.Url != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Url);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            } 
        }
        
        public async void AutoFillUsernameAndPassword()
        {    
            if (SelectedEntry != null)
            {
                HideWindow.Invoke();
                await PutTaskDelay(1000);
                if (SelectedEntry.Username != null)
                {
                    ForegroundWindowHandler.SetActiveWindow(ActiveWindow);
                    System.Windows.Clipboard.SetText(SelectedEntry.Username);
                    SendKeys.SendWait("^v");
                    System.Windows.Clipboard.Clear();
                    SendKeys.SendWait("{ENTER}");
                }
                await PutTaskDelay(3000);
                if (SelectedEntry.Password != null)
                {
                    System.Windows.Clipboard.SetText(SelectedEntry.Password);
                    SendKeys.SendWait("^v");                   
                    System.Windows.Clipboard.Clear();
                    SendKeys.SendWait("{ENTER}");
                }               
                LockDb();                      
            }
        }

        public async void AutoFillPassword()
        {
            if (SelectedEntry != null)
            {
                HideWindow.Invoke();
                await PutTaskDelay(1000);
                if (SelectedEntry.Password != null)
                {
                    System.Windows.Clipboard.SetText(SelectedEntry.Password);
                    SendKeys.SendWait("^v");
                    System.Windows.Clipboard.Clear();
                    SendKeys.SendWait("{ENTER}");
                }
                LockDb();
            }
        }

        #endregion

        #region Helpers

        private async void OpenAndDecrypt()
        {
            try
            {
                var loadedPasswords = PasswordEntryDb.DecryptAndDeserialize(DatabasePath, DbPassword);
                Passwords.Clear();
                foreach (var item in loadedPasswords) Passwords.Add(item);
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            catch  //System.Security.Cryptography.CryptographicException
            {
                var view = new MessageDialog()
                {
                    DataContext = new MessageDialogViewModel("Incorrect password or broken database.")
                };
                await DialogHost.Show(view, "MessageHost");
            }
        }

        private bool IsValidPath(string path)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);
                string root = Path.GetPathRoot(path);
                isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
            }
            catch { isValid = false; }
            return isValid;
        }

        private bool IsDbSelectedAndPasswordFilled()
        {
            try
            {
                if (DbPassword?.Length > 0 && DatabasePath != null)
                {
                    if (IsValidPath(DatabasePath) == true) return true;
                }
            }
            catch (System.ObjectDisposedException) { }
            return false;
        }

        private async Task PutTaskDelay(int ms)
        {
            await Task.Delay(ms);
        }

        #endregion

        #region Clean up

        ~PasswordEntryViewModel()
        {
            DbPassword.Dispose();
        }

        #endregion
    }
}
