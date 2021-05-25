﻿using System;
using System.Windows.Input;

namespace PasswordManagerWPF
{
    public class RelayCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter)
        {
            // Always execute
            return true;
        }

        public RelayCommand(Action _action)
        {
            action = _action;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
