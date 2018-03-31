using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4Remapper.Hooks
{
    public class WindowHook
    {
        #region ShowStreamingToolBar
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumThreadWindows(int threadId, EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;

            foreach (ProcessThread t in process.Threads)
            {
                windowHandle = FindWindowInThread(t.Id, compareTitle);
                if (windowHandle != IntPtr.Zero)
                {
                    break;
                }
            }

            return windowHandle;
        }

        private static IntPtr FindWindowInThread(int threadId, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;
            EnumThreadWindows(threadId, (hWnd, lParam) =>
            {
                StringBuilder text = new StringBuilder(200);
                GetWindowText(hWnd, text, 200);
                if (compareTitle(text.ToString()))
                {
                    windowHandle = hWnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);

            return windowHandle;
        }

        public static void ShowStreamingToolBar(Process process, bool show)
        {
            IntPtr streamingToolBar = FindWindowInProcess(process, title => title.Equals("StreamingToolBar"));
            if (streamingToolBar != IntPtr.Zero)
            {
                ShowWindow(streamingToolBar.ToInt32(), show ? SW_SHOW : SW_HIDE);
            }
        }
        #endregion

        #region IsProcessInForeground
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static bool IsProcessInForeground(Process process)
        {
            if (process == null)
                return false;
            if (process.HasExited)
                return false;

            // Check for focused window
            var activeWindow = GetForegroundWindow();
            if (activeWindow == IntPtr.Zero)
                return false;
            if (activeWindow != process.MainWindowHandle)
                return false;

            return true;
        }
        #endregion


    }
}