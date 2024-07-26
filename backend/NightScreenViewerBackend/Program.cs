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
                            string response = await ProcessMessageAsync(message);
                            await writer.WriteLineAsync(response);
                        }
                    }
                }
            }
        }

        static async Task<string> ProcessMessageAsync(string message)
        {
            switch (message)
            {
                case "autoOn":
                    return "Auto On enabled";
                case "autoOff":
                    return "Auto Off disabled";
                case "startBlackScreen":
                    return await Task.Run(() => screenManager.StartBlackScreen());
                case "stopBlackScreen":
                    return await Task.Run(() => screenManager.StopBlackScreen());
                default:
                    if (message.StartsWith("setOpacity:"))
                    {
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
}
