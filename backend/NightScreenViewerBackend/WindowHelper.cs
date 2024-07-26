using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class WindowHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        public static Screen GetScreenFromWindow(IntPtr hWnd)
        {
            IntPtr monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            if (GetMonitorInfo(monitor, ref monitorInfo))
            {
                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.Bounds.Left == monitorInfo.rcMonitor.Left &&
                        screen.Bounds.Top == monitorInfo.rcMonitor.Top &&
                        screen.Bounds.Width == (monitorInfo.rcMonitor.Right - monitorInfo.rcMonitor.Left) &&
                        screen.Bounds.Height == (monitorInfo.rcMonitor.Bottom - monitorInfo.rcMonitor.Top))
                    {
                        return screen;
                    }
                }
            }
            return Screen.PrimaryScreen;
        }

        public static bool IsForegroundWindowOnPrimaryScreen()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            Screen screen = GetScreenFromWindow(foregroundWindow);
            bool isOnPrimaryScreen = screen.Primary;

            // 输出调试信息到控制台
            Console.WriteLine($"Foreground Window: {foregroundWindow}, Screen: {screen.DeviceName}, On Primary: {isOnPrimaryScreen}");

            return isOnPrimaryScreen;
        }
    }
}
