using System;
using System.Drawing;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class BlackScreenForm
    {
        // 创建黑屏窗体
        public static Form CreateBlackScreenForm(Screen screen, double opacity)
        {
            var form = new Form
            {
                FormBorderStyle = FormBorderStyle.None, // 无边框窗体
                Bounds = screen.Bounds, // 设置窗体大小为屏幕大小
                BackColor = Color.Black, // 背景色为黑色
                Opacity = opacity, // 设置初始不透明度
                StartPosition = FormStartPosition.Manual, // 手动设置窗体位置
                ShowInTaskbar = false, // 不在任务栏显示
                TopMost = true // 窗体置顶
            };

            // 在窗体加载时设置窗体为分层窗体，并设置不透明度
            form.Load += (sender, e) =>
            {
                NativeMethods.SetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE, NativeMethods.GetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE) | NativeMethods.WS_EX_LAYERED);
                byte opacityByte = (byte)(opacity * 255);
                NativeMethods.SetLayeredWindowAttributes(form.Handle, 0, opacityByte, NativeMethods.LWA_ALPHA);
            };

            return form; // 返回创建的黑屏窗体
        }
    }
}
