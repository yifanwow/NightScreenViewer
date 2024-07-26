using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NightScreenViewerBackend
{
    public static class ScreenHelper
    {
        // 获取所有非主屏幕
        public static Screen[] GetNonPrimaryScreens(Screen[] screens, Screen mainScreen)
        {
            return screens.Where(s => s != mainScreen).ToArray(); // 返回所有非主屏幕
        }

        // 显示窗体
        public static void ShowForm(Form form)
        {
            var thread = new Thread(() =>
            {
                Application.Run(form); // 在新线程上运行窗体
            });
            thread.SetApartmentState(ApartmentState.STA); // 设置线程为单线程单元
            thread.Start(); // 启动线程
        }
    }
}
