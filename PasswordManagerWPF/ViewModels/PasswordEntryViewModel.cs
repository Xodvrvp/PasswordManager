using MaterialDesignThemes.Wpf;
using PasswordManagerCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

/// <summary>
/// View model for password enteries
/// </summary>
namespace PasswordManagerWPF
{
    public class PasswordEntryViewModel : BaseViewModel
    {
        #region Public properties

        // list of all entries
        public ObservableCollection<PasswordEntryModel> Passwords { get; set; } = new ObservableCollection<PasswordEntryModel>();
        // selected entry
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
        // pasword used to encrypt/decrypt database
        public SecureString DbPassword 
        { 
            get => _DbPassword; 
            set
            {
                if (_DbPassword == value) return;
                _DbPassword = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllowAccept));
            }
        }        
        // path of database
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

        // enables accept button if user has selected database and filled password
        public bool AllowAccept
        {
            get => IsDbSelectedAndPasswordFilled();
        }
        // visibility of PasswordEntryControl.xaml usercontrol
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
        // visibility of autofill buttons
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
        // visibility of password
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

        // password generator settings
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
        // visibility of password lenght slider, if none of above options are selected slider must be disabled 
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

        // action from WindowViewModel, allows this viewmodel to minimize window
        public Action HideWindow { get; set; }
        // name of a window where autofill should occur
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

        /// <summary>
        /// Opens OpenDbDialog dialog
        /// </summary>
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

        /// <summary>
        /// Opens SaveDbDialog dialog
        /// </summary>
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

        /// <summary>
        /// Saves entries to database
        /// </summary>
        /// <param name="dialogHostName"></param>
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

        /// <summary>
        /// Clears all entries and database password
        /// </summary>
        public void LockDb()
        {
            Passwords.Clear();
            DbPassword?.Dispose();
            if (PasswordControlVisible == true) ToggleControl();
        }         
        
        /// <summary>
        /// Opens system's file dialog
        /// </summary>
        private void OpenFileDialog()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Passwords database (*.pmdb)|*.pmdb";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) DatabasePath = openFile.FileName;
        }

        /// <summary>
        /// Opens system's save file dialog
        /// </summary>
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

        /// <summary>
        /// Adds new entry
        /// </summary>
        private void PwdEntryNew()
        {
            var newEntry = new PasswordEntryModel();
            Passwords.Add(newEntry);
            SelectedEntry = newEntry;
            PasswordControlVisible = true;
        }

        /// <summary>
        /// Deletes selected entry
        /// </summary>
        private void PwdEntryDelete()
        {
            Passwords.Remove(SelectedEntry);
            // if there are no entires left hide password edit control
            if (Passwords.Count == 0) PasswordControlVisible = false;
        }

        /// <summary>
        /// Toggles <see cref="PasswordEntryControl"/> visibility
        /// </summary>
        private void ToggleControl()
        {
            PasswordControlVisible ^= true;
        }

        /// <summary>
        /// Toggles password visibility
        /// </summary>
        private void ShowPasswordToggle()
        {
            if (SelectedEntry != null) PasswordVisible ^= true;
        }

        /// <summary>
        /// Generates random password
        /// </summary>
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

        /// <summary>
        /// Copies username from selected entry to clipboard
        /// </summary>
        private async void CopyUsername()
        {
            if (SelectedEntry.Username != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Username);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            }
        }

        /// <summary>
        /// Copies password from selected entry to clipboard
        /// </summary>
        private async void CopyPassword()
        {       
            if (SelectedEntry.Password != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Password);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            }
        }

        /// <summary>
        /// Copies URL from selected entry to clipboard
        /// </summary>
        private async void CopyUrl()
        {
            if (SelectedEntry.Url != null)
            {
                System.Windows.Clipboard.SetText(SelectedEntry.Url);
                await PutTaskDelay(10000);
                System.Windows.Clipboard.Clear();
            } 
        }
        
        /// <summary>
        /// Copies and pastes username and password from selected entry to <see cref="ActiveWindow"/>
        /// </summary>
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

        /// <summary>
        /// Copies and pastes password from selected entry to <see cref="ActiveWindow"/>
        /// </summary>
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

        /// <summary>
        /// Loads passwords from database
        /// </summary>
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

        /// <summary>
        /// Intercepts closing dialog event, if user clicked accept, saves entries to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandlerEncrypt(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter && parameter == true)
            {
                PasswordEntryDb.EncryptAndSerialize(DatabasePath, Passwords, DbPassword);
            }
        }

        /// <summary>
        /// Checks if database path is valid. Doesn't check if file exist!
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if user has selected database path and if has typed anything on the password box
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Waits <paramref name="ms"/> miliseconds. Doens't lock UI.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
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
