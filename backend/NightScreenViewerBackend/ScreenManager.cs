using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public class ScreenManager
    {
        private Form[] blackScreenForms;
        private SynchronizationContext mainContext;

        public ScreenManager()
        {
            mainContext = SynchronizationContext.Current;
        }

        public string StartBlackScreen()
        {
            var screens = Screen.AllScreens;
            var mainScreen = Screen.PrimaryScreen;
            var nonPrimaryScreens = Array.FindAll(screens, s => s != mainScreen);

            blackScreenForms = new Form[nonPrimaryScreens.Length];
            for (int i = 0; i < nonPrimaryScreens.Length; i++)
            {
                blackScreenForms[i] = CreateBlackScreenForm(nonPrimaryScreens[i], 0.9); // 90% 不透明度
                ShowForm(blackScreenForms[i]);
            }

            return "Black Screen started";
        }

        public string StopBlackScreen()
        {
            if (blackScreenForms != null)
            {
                for (int i = 0; i < blackScreenForms.Length; i++)
                {
                    CloseForm(blackScreenForms[i]);
                }
                blackScreenForms = null; // 清空数组引用，防止复用
            }

            return "Black Screen stopped";
        }

        private Form CreateBlackScreenForm(Screen screen, double opacity)
        {
            var form = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                Bounds = screen.Bounds,
                BackColor = Color.Black,
                Opacity = opacity, // 90% 不透明度
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost = true
            };

            form.Load += (sender, e) =>
            {
                SetWindowLong(form.Handle, GWL_EXSTYLE, GetWindowLong(form.Handle, GWL_EXSTYLE) | WS_EX_LAYERED);
                byte opacityByte = (byte)(opacity * 255); // 将不透明度转换为字节值
                SetLayeredWindowAttributes(form.Handle, 0, opacityByte, LWA_ALPHA);
            };

            return form;
        }

        private void ShowForm(Form form)
        {
            var thread = new Thread(() =>
            {
                mainContext = SynchronizationContext.Current;
                Application.Run(form);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void CloseForm(Form form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new Action(() => form.Close()));
            }
            else
            {
                form.Close();
            }

            form.Dispose();
            Console.WriteLine("Closed form on screen: " + form.Bounds);
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
    }
}
