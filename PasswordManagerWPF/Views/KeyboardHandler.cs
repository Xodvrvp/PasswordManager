using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace PasswordManagerWPF
{
    public class KeyboardHandler
    {
        public const int WM_HOTKEY = 0x0312;
        public const int VIRTUALKEYCODE_FOR_CAPS_LOCK = 0x79;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly Window _mainWindow;
        private Action _action;
        WindowInteropHelper _host;
        public KeyboardHandler(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);

            SetupHotKey(_host.Handle);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public KeyboardHandler(Window mainWindow, Action action)
        {
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);
            _action = action;
            SetupHotKey(_host.Handle);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_RunAction;
        }

        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY)
            {
                throw new NotImplementedException();
            }
        }

        void ComponentDispatcher_RunAction(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY)
            {
                _action.Invoke();
            }
        }

        private void SetupHotKey(IntPtr handle)
        {
            RegisterHotKey(handle, GetType().GetHashCode(), 0, VIRTUALKEYCODE_FOR_CAPS_LOCK);
        }

        public void Dispose()
        {
            UnregisterHotKey(_host.Handle, GetType().GetHashCode());
        }
    }
}
