using System;
using System.Runtime.InteropServices;

namespace NightScreenViewerBackend
{
    public static class NativeMethods
    {
        public const int GWL_EXSTYLE = -20; // 获取窗体扩展样式
        public const int WS_EX_LAYERED = 0x80000; // 分层窗体样式
        public const int LWA_ALPHA = 0x2; // 透明度属性
        public const int WS_EX_TRANSPARENT = 0x20; // 透明点击穿透

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong); // 设置窗体长整数值

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex); // 获取窗体长整数值

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags); // 设置分层窗体属性
    }
}
