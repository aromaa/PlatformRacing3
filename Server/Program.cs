using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Commands.Executors;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Platform_Racing_3_Server.Extensions;

namespace Platform_Racing_3_Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
			Console.Title = "Platform Racing 3 Server";

			try
            {
				XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config")); //Setup log4net logging
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to setup logging! {ex}");
			}

			Program.CreateHostBuilder(args)
	               .Build()
	               .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
	        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
	                 .ConfigurePlatformRacingServerDefaults();
    }
}
