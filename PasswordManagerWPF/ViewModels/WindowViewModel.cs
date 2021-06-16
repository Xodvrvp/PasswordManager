using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManagerWPF
{
    /// <summary>
    /// View Model for custom window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {
            Window = window;
            Page = new PasswordsPage() 
            { 
                DataContext = new PasswordEntryViewModel() 
            };
            KeyboardHandler kh = new KeyboardHandler(Window, new Action(SelectPassword));
        }

        #endregion

        #region Public properties

        public Page Page
        {
            get => page;
            set
            {
                if (page == value)
                    return;
                page = value;
                OnPropertyChanged();
            }
        }
        public Window Window
        {
            get => window;
            set
            {
                if (window == value)
                    return;
                window = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Size of resize border
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        /// Height of title bar
        /// </summary>
        public int TitleHeight { get; set; } = 40;

        #endregion

        #region Private fields

        private Window window;
        private Page page;
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        #endregion

        #region Commands

        public ICommand MinimizeCommand => new RelayCommand(MinimizeToTray);
        public ICommand MaximizeCommand => new RelayCommand(() => Window.WindowState ^= WindowState.Maximized);
        public ICommand CloseCommand => new RelayCommand(CloseWindow);

        #endregion

        #region Methods

        private void MinimizeToTray()
        {
            var datacontext = (PasswordEntryViewModel)Page.DataContext;
            datacontext.LockDb();            
            window.Hide();
            if (notifyIcon == null)
            {
                notifyIcon = new System.Windows.Forms.NotifyIcon();
                notifyIcon.Icon = new System.Drawing.Icon(Properties.Resources.IconNotify);
            }           
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    window.Show();
                    datacontext.OpenDb();
                    notifyIcon.Visible = false;
                };
        }

        private void SelectPassword()
        {
            var datacontext = (PasswordEntryViewModel)Page.DataContext;
            datacontext.ActiveWindow = PasswordManagerCore.ForegroundWindowHandler.GetActiveWindow();
            datacontext.SelectionMode = true;
            datacontext.HideWindow = new Action(HideWindow);
            window.Show();
            datacontext.OpenDb();
        }

        private void HideWindow()
        {
            window.Hide();
        }

        private void CloseWindow()
        {
            if(notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Icon = null;
                notifyIcon.Dispose();
            }           
            Window.Close();
        }

        #endregion
    }
}

