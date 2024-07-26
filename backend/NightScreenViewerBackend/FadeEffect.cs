using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class FadeEffect
    {
        public static async void FadeIn(Form form, double targetOpacity)
        {
            await Task.Delay(500); // Ensure form is visible
            for (double opacity = form.Opacity; opacity <= targetOpacity; opacity += 0.05)
            {
                form.Invoke(new Action(() => form.Opacity = opacity));
                await Task.Delay(50);
            }
        }

        public static async void FadeOutAndClose(Form form)
        {
            for (double opacity = form.Opacity; opacity > 0; opacity -= 0.03)
            {
                form.Invoke(new Action(() => form.Opacity = opacity));
                await Task.Delay(50);
            }
            form.Invoke(new Action(() => form.Close()));
            form.Dispose();
            Console.WriteLine("Closed form on screen: " + form.Bounds);
        }

        public static async void AdjustOpacity(Form form, double targetOpacity)
        {
            double step = form.Opacity < targetOpacity ? 0.05 : -0.05;
            while (Math.Abs(form.Opacity - targetOpacity) > 0.05)
            {
                double newOpacity = form.Opacity + step;
                form.Invoke(new Action(() => form.Opacity = newOpacity));
                await Task.Delay(50);
            }
            form.Invoke(new Action(() => form.Opacity = targetOpacity));
        }
    }
}
