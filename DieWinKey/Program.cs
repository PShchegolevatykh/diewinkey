using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DieWinKey
{
    static class Program
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_MOUSEWHEEL = 0x020A;
        private static IntPtr _keyboardHookID = IntPtr.Zero;
        private static IntPtr _mouseHookID = IntPtr.Zero;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                _keyboardHookID = Intercepter.SetKeyboardHook(KeyboardHookCallback);
                _mouseHookID = Intercepter.SetMouseHook(MouseHookCallback);
            }
            catch
            {
                if (_keyboardHookID != IntPtr.Zero)
                {
                    Intercepter.UnhookWindowsHookEx(_keyboardHookID);
                }
                if (_mouseHookID != IntPtr.Zero)
                {
                    Intercepter.UnhookWindowsHookEx(_mouseHookID);
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                if (key == Keys.LWin || key == Keys.RWin)
                {
                    return (IntPtr)1;
                }
            }
            return Intercepter.CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        }

        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_MOUSEWHEEL)
            {
                return (IntPtr)1;

            }

            return Intercepter.CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }
    }
}
