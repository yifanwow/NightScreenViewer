using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public class ScreenManager
    {
        private Form[] blackScreenForms; // 存储黑屏窗体的数组
        private SynchronizationContext mainContext; // 同步上下文，用于在主线程上执行代码

        public ScreenManager()
        {
            mainContext = SynchronizationContext.Current; // 获取当前同步上下文
        }

        // 启动黑屏功能
        public string StartBlackScreen()
        {
            var screens = Screen.AllScreens; // 获取所有屏幕
            var mainScreen = Screen.PrimaryScreen; // 获取主屏幕
            var nonPrimaryScreens = ScreenHelper.GetNonPrimaryScreens(screens, mainScreen); // 获取非主屏幕

            blackScreenForms = new Form[nonPrimaryScreens.Length]; // 初始化黑屏窗体数组
            for (int i = 0; i < nonPrimaryScreens.Length; i++)
            {
                blackScreenForms[i] = BlackScreenForm.CreateBlackScreenForm(nonPrimaryScreens[i], 0.5); // 初始不透明度设置为0.5
                ScreenHelper.ShowForm(blackScreenForms[i]); // 显示窗体
            }

            // 淡入效果，增加不透明度到0.9
            foreach (var form in blackScreenForms)
            {
                FadeEffect.FadeIn(form, 0.9); // 淡入到90%不透明度
            }

            return "Black Screen started"; // 返回启动信息
        }

        // 停止黑屏功能
        public string StopBlackScreen()
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.FadeOutAndClose(form); // 淡出并关闭窗体
                }
                blackScreenForms = null; // 清空黑屏窗体数组
            }

            return "Black Screen stopped"; // 返回停止信息
        }

        // 调整黑屏窗体的不透明度
        public string SetOpacity(double opacity)
        {
            if (blackScreenForms != null)
            {
                foreach (var form in blackScreenForms)
                {
                    FadeEffect.AdjustOpacity(form, opacity); // 调整不透明度
                }
                return $"Opacity adjusted to {opacity * 100}%"; // 返回调整后的不透明度信息
            }
            return "No black screens to adjust."; // 返回没有黑屏窗体的信息
        }
    }
}
