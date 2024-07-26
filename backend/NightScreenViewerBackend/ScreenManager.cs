using System;
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
            var nonPrimaryScreens = ScreenHelper.GetNonPrimaryScreens(screens, mainScreen);

            blackScreenForms = new Form[nonPrimaryScreens.Length];
            for (int i = 0; i < nonPrimaryScreens.Length; i++)
            {
                blackScreenForms[i] = BlackScreenForm.CreateBlackScreenForm(nonPrimaryScreens[i], 0.5); // Initial opacity is set to 0.5
                ScreenHelper.ShowForm(blackScreenForms[i]);
            }

            foreach (var form in blackScreenForms)
            {
                FadeEffect.FadeIn(form, 0.9); // Fade to 90% opacity
            }

            return "Black Screen started";
        }

        public string StopBlackScreen()
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.FadeOutAndClose(form);
                }
                blackScreenForms = null;
            }

            return "Black Screen stopped";
        }

        public string SetOpacity(double opacity)
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.AdjustOpacity(form, opacity);
                }
                return $"Opacity adjusted to {opacity * 100}%";
            }
            return "No black screens to adjust.";
        }
    }
}
