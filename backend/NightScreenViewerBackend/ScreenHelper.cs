using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class ScreenHelper
    {
        public static Screen[] GetNonPrimaryScreens(Screen[] screens, Screen mainScreen)
        {
            return screens.Where(s => s != mainScreen).ToArray();
        }

        public static void ShowForm(Form form)
        {
            var thread = new Thread(() =>
            {
                Application.Run(form);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
