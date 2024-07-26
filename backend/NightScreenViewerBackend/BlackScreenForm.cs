using System;
using System.Drawing;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class BlackScreenForm
    {
        public static Form CreateBlackScreenForm(Screen screen, double opacity)
        {
            var form = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                Bounds = screen.Bounds,
                BackColor = Color.Black,
                Opacity = opacity,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost = true
            };

            form.Load += (sender, e) =>
            {
                NativeMethods.SetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE, NativeMethods.GetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE) | NativeMethods.WS_EX_LAYERED);
                byte opacityByte = (byte)(opacity * 255);
                NativeMethods.SetLayeredWindowAttributes(form.Handle, 0, opacityByte, NativeMethods.LWA_ALPHA);
            };

            return form;
        }
    }
}
