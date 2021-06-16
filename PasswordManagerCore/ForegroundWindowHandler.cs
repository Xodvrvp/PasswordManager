using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace PasswordManagerCore
{
    public static class ForegroundWindowHandler
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        public static string GetActiveWindow()
        {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);

            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public static void SetActiveWindow(string appName)
        {
            var proc = Process.GetProcessesByName(appName).FirstOrDefault();
            if (proc != null)
            {
                var handle = proc.MainWindowHandle;
                SetForegroundWindow(handle);
            }
        }
    }
}
