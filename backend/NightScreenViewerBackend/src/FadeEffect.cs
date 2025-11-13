using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class FadeEffect
    {
        // 调整窗体不透明度
        public static async Task AdjustOpacityAsync(
            Form form,
            double targetOpacity,
            CancellationToken cancellationToken
        )
        {
            double step = form.Opacity < targetOpacity ? 0.03 : -0.03;
            IntPtr hWnd = form.Handle;
            while (Math.Abs(form.Opacity - targetOpacity) > 0.03)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return; // 如果取消请求被发出，则终止任务
                }
                double newOpacity = form.Opacity + step;
                form.Invoke(
                    new Action(() =>
                    {
                        form.Opacity = newOpacity;
                        // 确保保持透明点击穿透属性
                        NativeMethods.SetWindowLong(
                            hWnd,
                            NativeMethods.GWL_EXSTYLE,
                            NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE)
                                | NativeMethods.WS_EX_LAYERED
                                | NativeMethods.WS_EX_TRANSPARENT
                        );
                        byte opacityByte = (byte)(newOpacity * 255);
                        NativeMethods.SetLayeredWindowAttributes(
                            hWnd,
                            0,
                            opacityByte,
                            NativeMethods.LWA_ALPHA
                        );
                    })
                );
                await Task.Delay(50, cancellationToken); // 等待50毫秒，支持取消
            }
            form.Invoke(
                new Action(() =>
                {
                    form.Opacity = targetOpacity; // 设置最终的不透明度
                    // 确保保持透明点击穿透属性
                    NativeMethods.SetWindowLong(
                        hWnd,
                        NativeMethods.GWL_EXSTYLE,
                        NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE)
                            | NativeMethods.WS_EX_LAYERED
                            | NativeMethods.WS_EX_TRANSPARENT
                    );
                    byte opacityByte = (byte)(targetOpacity * 255);
                    NativeMethods.SetLayeredWindowAttributes(
                        hWnd,
                        0,
                        opacityByte,
                        NativeMethods.LWA_ALPHA
                    );
                })
            );
        }

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
    }
}
