using Microsoft.Extensions.Hosting;
using PlatformRacing3.Server.Extensions;

namespace PlatformRacing3.Server;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		Console.Title = "Platform Racing 3 Server";

		await Program.CreateHostBuilder(args)
		             .Build()
		             .RunAsync();
	}

	private static IHostBuilder CreateHostBuilder(string[] args) =>
		Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
		         .ConfigurePlatformRacingServerDefaults();
}