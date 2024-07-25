using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace NightScreenViewerBackend
{
    class Program
    {
         private static ScreenManager screenManager = new ScreenManager();
        static async Task Main(string[] args)
        {
            Console.WriteLine("NightScreenViewer Backend started...");
            await ListenForMessages();
        }

        static async Task ListenForMessages()
        {
            while (true)
            {
                using (var pipeServer = new NamedPipeServerStream("NightScreenViewerPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                {
                    Console.WriteLine("Waiting for connection...");
                    await pipeServer.WaitForConnectionAsync();
                    Console.WriteLine("Client connected.");

                    using (var reader = new StreamReader(pipeServer))
                    using (var writer = new StreamWriter(pipeServer) { AutoFlush = true })
                    {
                        string message;
                        while ((message = await reader.ReadLineAsync()) != null)
                        {
                            Console.WriteLine($"Received message: {message}");
                            string response = ProcessMessage(message);
                            await writer.WriteLineAsync(response);
                        }
                    }
                }
            }
        }

        static string ProcessMessage(string message)
        {
            // 在这里处理从前端接收到的消息，并返回响应
            if (message == "autoOn")
            {
                // 启用自动黑屏逻辑
                return "Auto On enabled";
            }
            else if (message == "autoOff")
            {
                // 禁用自动黑屏逻辑
                return "Auto Off disabled";
            }
            else if (message == "startBlackScreen")
            {
                // 开始黑屏逻辑
                return screenManager.StartBlackScreen();
            }
            else if (message == "stopBlackScreen")
            {
                // 停止黑屏逻辑
                return screenManager.StopBlackScreen();
            }
            else if (message.StartsWith("setOpacity:"))
            {
                // 设置黑屏的不透明度
                string opacityValue = message.Split(':')[1];
                return $"Opacity set to {opacityValue}%";
            }
            else
            {
                return "Unknown command";
            }
        }
    }
}
