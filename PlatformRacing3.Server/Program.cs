using System;
using Microsoft.Extensions.Hosting;
using PlatformRacing3.Server.Extensions;

namespace PlatformRacing3.Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
			Console.Title = "Platform Racing 3 Server";

			Program.CreateHostBuilder(args)
	               .Build()
	               .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
	        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
	                 .ConfigurePlatformRacingServerDefaults();
    }
}
