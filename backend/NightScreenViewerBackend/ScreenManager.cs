using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public class ScreenManager
    {
        private static Form[]? blackScreenForms; // 声明为nullable类型
        private SynchronizationContext? mainContext; // 声明为nullable类型

        public ScreenManager()
        {
            mainContext = SynchronizationContext.Current; // 获取当前同步上下文
        }

        public string StartBlackScreen()
        {
            var screens = Screen.AllScreens;
            var mainScreen =
                Screen.PrimaryScreen
                ?? throw new InvalidOperationException("Primary screen not found."); // 确保mainScreen不为null
            var nonPrimaryScreens = ScreenHelper.GetNonPrimaryScreens(screens, mainScreen);

            blackScreenForms = new Form[nonPrimaryScreens.Length];
            for (int i = 0; i < nonPrimaryScreens.Length; i++)
            {
                blackScreenForms[i] = BlackScreenForm.CreateBlackScreenForm(
                    nonPrimaryScreens[i],
                    0.5
                ); // 初始不透明度设置为0.5
                ScreenHelper.ShowForm(blackScreenForms[i]);
            }

            foreach (var form in blackScreenForms)
            {
                FadeEffect.FadeIn(form, 0.9); // 淡入到90%不透明度
            }

            // 启动焦点变化检测
            StartFocusWatcher();

            return "Black Screen started";
        }

        public string StopBlackScreen()
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.FadeOutAndClose(form); // 淡出并关闭窗体
                }
                blackScreenForms = null;
            }

            // 停止焦点变化检测
            StopFocusWatcher();

            return "Black Screen stopped";
        }

        public string SetOpacity(double opacity)
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.AdjustOpacity(form, opacity); // 调整不透明度
                }
                return $"Opacity adjusted to {opacity * 100}%";
            }
            return "No black screens to adjust.";
        }

        // 刷新黑屏窗体的置顶属性
        public static void RefreshBlackScreenForms()
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    form.Invoke(
                        new Action(() =>
                        {
                            form.TopMost = false;
                            form.TopMost = true;
                        })
                    );
                }
                Console.WriteLine("Refreshed black screen forms.");
            }
        }

        private Task? focusWatcherTask;
        private CancellationTokenSource? focusWatcherCts;

        private void StartFocusWatcher()
        {
            focusWatcherCts = new CancellationTokenSource();
            var token = focusWatcherCts.Token;
            focusWatcherTask = Task.Run(
                async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(3000); // 检测间隔，可以调整
                        foreach (var form in blackScreenForms)
                        {
                            form.Invoke(
                                new Action(() =>
                                {
                                    form.TopMost = false;
                                })
                            );
                        }
                        if (WindowHelper.IsForegroundWindowOnPrimaryScreen())
                        {
                            RefreshBlackScreenForms();
                        }
                    }
                },
                token
            );
        }

        private void StopFocusWatcher()
        {
            if (focusWatcherCts != null)
            {
                focusWatcherCts.Cancel();
                focusWatcherCts = null;
            }
        }
    }
}
