using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public class ScreenManager
    {
        private Dictionary<Screen, Form> screenForms = new Dictionary<Screen, Form>();

        public string StartBlackScreen()
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (screen == Screen.PrimaryScreen) continue;

                var form = new Form
                {
                    BackColor = Color.Black,
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    WindowState = FormWindowState.Maximized,
                    Bounds = screen.Bounds,
                    Opacity = 0.5 // 设置初始的不透明度为 50%
                };

                form.Show();
                screenForms[screen] = form;
            }

            return "Black Screen started";
        }

        public string StopBlackScreen()
        {
            foreach (var form in screenForms.Values)
            {
                form.Close();
            }

            screenForms.Clear();
            return "Black Screen stopped";
        }
    }
}
