using System;
using System.Windows.Forms;
using System.Drawing;

namespace NightScreenViewerBackend
{
    public static class MirrorMode
    {
        private static bool isMirrorModeEnabled = false;

        public static async Task<string> CheckAndEnableMirrorModeAsync()
        {
            var screens = Screen.AllScreens;

            if (screens.Length >= 3)
            {
                EnableMirrorMode();
                return "Mirror mode enabled, screen count: " + screens.Length;
            }
            else
            {
                DisableMirrorMode();
                return "Mirror mode disabled, screen count: " + screens.Length;
            }
        }

        public static void EnableMirrorMode()
        {
            isMirrorModeEnabled = true;
            Console.WriteLine("Mirror mode enabled.");
        }

        public static void DisableMirrorMode()
        {
            isMirrorModeEnabled = false;
            Console.WriteLine("Mirror mode disabled.");
        }

        public static void DisplayLabelOnScreen(Screen screen, string label)
        {
            var form = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                Bounds = screen.Bounds,
                BackColor = Color.Black,
                TransparencyKey = Color.Black, // 使该颜色完全透明
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost = true
            };

            var labelControl = new Label
            {
                Text = label,
                Font = new Font("Arial", 48, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            form.Controls.Add(labelControl);
            form.Shown += async (sender, args) =>
            {
                labelControl.Location = new Point(
                    form.ClientSize.Width / 2 - labelControl.Size.Width / 2,
                    form.ClientSize.Height / 2 - labelControl.Size.Height / 2
                );

                // 5秒后自动关闭
                await Task.Delay(5000);
                if (!form.IsDisposed)
                {
                    form.Invoke(new Action(() => form.Close()));
                }
            };

            Application.Run(form);
        }

        public static bool IsMirrorModeEnabled()
        {
            return isMirrorModeEnabled;
        }
    }
}
