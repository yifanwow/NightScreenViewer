using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class FadeEffect
    {
        // 淡入效果
        // public static async void FadeIn(Form form, double targetOpacity)
        // {
        //     await Task.Delay(500); // 确保窗体可见
        //     for (double opacity = form.Opacity; opacity <= targetOpacity; opacity += 0.05)
        //     {
        //         form.Invoke(new Action(() => form.Opacity = opacity)); // 更新窗体不透明度
        //         await Task.Delay(50); // 等待50毫秒
        //     }
        // }

        // 淡出并关闭窗体
        public static async void FadeOutAndClose(Form form)
        {
            for (double opacity = form.Opacity; opacity > 0; opacity -= 0.03)
            {
                form.Invoke(new Action(() => form.Opacity = opacity)); // 更新窗体不透明度
                await Task.Delay(50); // 等待50毫秒
            }
            form.Invoke(new Action(() => form.Close())); // 关闭窗体
            form.Dispose(); // 释放窗体资源
            Console.WriteLine("Closed form on screen: " + form.Bounds); // 输出关闭窗体的信息
        }

        // 调整窗体不透明度
        public static async void AdjustOpacity(Form form, double targetOpacity)
        {
            double step = form.Opacity < targetOpacity ? 0.03 : -0.03;
            while (Math.Abs(form.Opacity - targetOpacity) > 0.03)
            {
                double newOpacity = form.Opacity + step;
                form.Invoke(new Action(() => form.Opacity = newOpacity)); // 更新窗体不透明度
                await Task.Delay(50); // 等待50毫秒
            }
            form.Invoke(new Action(() => form.Opacity = targetOpacity)); // 设置最终的不透明度
        }
    }
}
