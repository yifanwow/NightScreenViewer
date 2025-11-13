using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class FullscreenDetector
    {
        private static Task? detectionTask;
        private static CancellationTokenSource? cts;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private static IntPtr desktopHandle = GetDesktopWindow();
        private static IntPtr shellHandle = GetShellWindow();

        public static void StartDetection(Action onEnterFullscreen, Action onExitFullscreen)
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            detectionTask = Task.Run(
                async () =>
                {
                    bool wasFullscreen = false;

                    while (!token.IsCancellationRequested)
                    {
                        bool isFullscreen = AreApplicationFullScreen();

                        if (isFullscreen)
                        {
                            onEnterFullscreen();
                        }
                        else if (!isFullscreen)
                        {
                            onExitFullscreen();
                        }

                        wasFullscreen = isFullscreen;
                        await Task.Delay(1700); // 检测间隔
                    }
                },
                token
            );
        }

        public static void StopDetection()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
        }

        public static bool AreApplicationFullScreen()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
                return false;

            if (hWnd == desktopHandle || hWnd == shellHandle)
                return false;

            GetWindowRect(hWnd, out RECT appBounds);
            var screenBounds = Screen.FromHandle(hWnd).Bounds;

            return (appBounds.Bottom - appBounds.Top) == screenBounds.Height
                && (appBounds.Right - appBounds.Left) == screenBounds.Width;
        }
    }
}
