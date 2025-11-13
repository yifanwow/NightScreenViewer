using System;
using System.Drawing;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public class GlowForm : Form
    {
        public IntPtr Hwnd => Handle;

        public GlowForm(Screen target)
        {
            FormBorderStyle = FormBorderStyle.None;
            Bounds = new Rectangle(
                target.Bounds.X,
                target.Bounds.Y,
                target.Bounds.Width,
                target.Bounds.Height
            );
            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Black;
            Opacity = 1.0;

            // ✅ 启用绘制样式
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_LAYERED = 0x80000; // ✅ 仅保留 layered，去掉 transparent
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                return cp;
            }
        }
    }
}
