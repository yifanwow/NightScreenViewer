using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class TheaterMode
    {
        private static readonly List<(GlowForm form, GlowRenderer renderer)> _instances = new();

        public static void Enable()
        {
            Disable(); // 幂等操作

            var all = Screen.AllScreens;
            var main = Screen.PrimaryScreen!;
            // 找主屏左右相邻屏幕
            var left = all.FirstOrDefault(s => s.Bounds.Right == main.Bounds.Left);
            var right = all.FirstOrDefault(s => s.Bounds.Left == main.Bounds.Right);

            foreach (var target in new[] { left, right })
            {
                if (target == null)
                    continue;

                var form = new GlowForm(target);
                ScreenHelper.ShowForm(form); // 在独立线程显示窗体

                form.Shown += async (s, e) =>
                {
                    await Task.Delay(100); // ✅ 等待句柄完全初始化

                    var renderer = new GlowRenderer(form);
                    try
                    {
                        renderer.Initialize();
                        renderer.Start();
                        _instances.Add((form, renderer));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[TheaterMode] Renderer init failed: {ex.Message}");
                    }
                };
            }

            Console.WriteLine("Theater Mode enabled.");
        }

        public static void Disable()
        {
            foreach (var (form, renderer) in _instances.ToArray())
            {
                try
                {
                    renderer.Dispose();
                }
                catch { }
                try
                {
                    if (!form.IsDisposed)
                        form.Invoke(new Action(() => form.Close()));
                }
                catch { }
            }
            _instances.Clear();
            Console.WriteLine("Theater Mode disabled.");
        }
    }
}
