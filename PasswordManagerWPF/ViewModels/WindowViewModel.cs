﻿using System.Windows;
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
            Page = new PasswordsPage();
            MinimizeCommand = new RelayCommand(() => Window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => Window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => Window.Close());
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

        #region Private member
        private Window window;
        private Page page;
        #endregion

        #region Commands
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion
    }
}
