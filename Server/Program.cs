using log4net;
using log4net.Config;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Commands.Executors;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Platform_Racing_3_Server
{
    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly PlatformRacing3Server Instance = new();

        private static void Main(string[] args)
        {
            Console.Title = "Platform Racing 3 Server";

            try
            {
                XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config")); //Setup log4net logging
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to setup logging! {ex}");
            }

            Program.Logger.Info("Starting up server...");
            Program.Instance.Init();
            Program.Logger.Info("Server is ready!");

            ConsoleCommandExecutor consoleExecutor = new();

            while (true)
            {
                string line = Console.ReadLine();
                string[] commmandArgs = line.Split(' ');

                if (!PlatformRacing3Server.CommandManager.Execte(consoleExecutor, commmandArgs[0], commmandArgs.AsSpan().Slice(1, commmandArgs.Length - 1)))
                {
                    Console.WriteLine($"Unknown command!");
                }
            }
        }

        internal static void Shutdown()
        {
            Program.Instance.Shutdown();
        }
    }
}
