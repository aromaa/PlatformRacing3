using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Server.Config;
using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Commands;
using PlatformRacing3.Server.Game.Communication.Messages;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;
using PlatformRacing3.Server.Host;
using PlatformRacing3.Server.Host.Implementation;

namespace PlatformRacing3.Server.Extensions;

public static class HostBuilderExtensions
{
	public static IHostBuilder ConfigurePlatformRacingServerDefaults(this IHostBuilder builder)
	{
		return builder.ConfigurePlatformRacingServerDefaults(_ => { });
	}

	public static IHostBuilder ConfigurePlatformRacingServerDefaults(this IHostBuilder builder, Action<IServerHostBuilder> configure)
	{
		return builder.ConfigurePlatformRacingServer(hostBuilder =>
		{
			hostBuilder.ConfigureServices((configure, services) =>
			{
				services.AddSingleton<PlatformRacing3Server>();
				services.AddSingleton<CommandManager>();
				services.AddSingleton<MatchManager>();
				services.AddSingleton<PacketManager>();
				services.AddSingleton<ChatRoomManager>();
				services.AddSingleton<MatchListingManager>();
				services.AddSingleton<PacketManager>();
				services.AddSingleton<ServerManager>();
				services.AddSingleton<CampaignManager>();
				services.AddSingleton<ClientManager>();

				services.Configure<ServerConfig>(configure);
			});

			configure(hostBuilder);
		});
	}

	public static IHostBuilder ConfigurePlatformRacingServer(this IHostBuilder builder, Action<IServerHostBuilder> configure)
	{
		builder.ConfigureHostConfiguration(config =>
		{
			config.AddJsonFile("settings.json");
		});

		ServerHostBuilder hostBuilder = new(builder);
		configure(hostBuilder);

		builder.ConfigureServices((context, services) =>
		{
			services.AddHostedService<ServerHostService>();
		});

		return builder;
	}
}