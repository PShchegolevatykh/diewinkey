using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DieWinKey
{

    
    static class Program
    {
        private const int WM_KEYDOWN = 0x0100;
        private static KeyIntercepter.LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                _hookID = KeyIntercepter.SetHook(_proc);
            }
            catch
            {
                if (_hookID != IntPtr.Zero)
                {
                    KeyIntercepter.UnhookWindowsHookEx(_hookID);
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
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
            return KeyIntercepter.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
