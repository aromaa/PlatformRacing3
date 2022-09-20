using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Discord.Config;
using PlatformRacing3.Discord.Core;

namespace PlatformRacing3.Discord;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		LoggerUtil.LoggerFactory = NullLoggerFactory.Instance;

		DiscordBotConfig config = JsonConvert.DeserializeObject<DiscordBotConfig>(File.ReadAllText("settings.json"));

		DatabaseConnection.Init(config);
		RedisConnection.Init(config);

		IServiceProvider serviceProvider = new ServiceCollection()
			.AddSingleton(config)
			.AddSingleton<ServerManager>()
			.AddSingleton<CampaignManager>()
			.AddSingleton<DiscordBot>()
			.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>))
			.BuildServiceProvider();

		await serviceProvider.GetRequiredService<ServerManager>().LoadServersAsync();
		await serviceProvider.GetRequiredService<CampaignManager>().LoadCampaignTimesAsync();
		await serviceProvider.GetRequiredService<CampaignManager>().LoadPrizesAsync();
		await serviceProvider.GetRequiredService<DiscordBot>().SetupDiscordBot();

		Console.WriteLine("Ready!");

		await Task.Delay(-1);
	}
}