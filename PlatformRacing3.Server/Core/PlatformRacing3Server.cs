using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using Net.Sockets.Listener;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Server.Config;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Handlers;
using PlatformRacing3.Server.Game.Communication.Managers;
using StackExchange.Redis;

namespace PlatformRacing3.Server.Core;

internal sealed class PlatformRacing3Server : PlatformRacing3.Server.API.Core.PlatformRacing3
{
	public const uint PROTOCOL_VERSION = 24;

	private static Stopwatch StartTime { get; set; }

	public static ServerConfig ServerConfig { get; set; }
        
	public static DiscordWebhookClient DiscordChatWebhook { get; private set; }
	public static DiscordWebhookClient DiscordNotificationsWebhook { get; private set; }

	private readonly IServiceProvider serviceProvider;

	private readonly ServerManager serverManager;
	private readonly ClientManager clientManager;

	private readonly CampaignManager campaignManager;

	private IListener listener;

	public PlatformRacing3Server(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, ServerManager serverManager, ClientManager clientManager, CampaignManager campaignManager)
	{
		LoggerUtil.LoggerFactory = loggerFactory;

		this.serviceProvider = serviceProvider;

		this.serverManager = serverManager;
		this.clientManager = clientManager;

		this.campaignManager = campaignManager;
	}

	internal async Task Init()
	{
		PlatformRacing3Server.StartTime = Stopwatch.StartNew();

		PlatformRacing3Server.ServerConfig = await JsonSerializer.DeserializeAsync<ServerConfig>(File.OpenRead("settings.json"));

		RedisConnection.Init(PlatformRacing3Server.ServerConfig);
		DatabaseConnection.Init(PlatformRacing3Server.ServerConfig);
            
		await this.serverManager.LoadServersAsync();
            
		await this.campaignManager.LoadCampaignTimesAsync();
		await this.campaignManager.LoadPrizesAsync();

		if (PlatformRacing3Server.ServerConfig.DiscordChatWebhookId != 0)
		{
			PlatformRacing3Server.DiscordChatWebhook = new DiscordWebhookClient(PlatformRacing3Server.ServerConfig.DiscordChatWebhookId, PlatformRacing3Server.ServerConfig.DiscordChatWebhookToken);
		}

		if (PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookId != 0)
		{
			PlatformRacing3Server.DiscordNotificationsWebhook = new DiscordWebhookClient(PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookId, PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookToken);
		}

		BytePacketManager bytePacketManager = new(this.serviceProvider);

		this.listener = IListener.CreateTcpListener(new IPEndPoint(IPAddress.Parse(PlatformRacing3Server.ServerConfig.BindIp), PlatformRacing3Server.ServerConfig.BindPort), socket =>
		{
			socket.Pipeline.AddHandlerFirst(new SplitPacketHandler(bytePacketManager, new ClientSession(socket)));
			socket.Pipeline.AddHandlerFirst(new FlashSocketPolicyRequestHandler());
		}, this.serviceProvider);

		_ = UpdateStatus();
	}

	private async Task UpdateStatus()
	{
		PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

		while (await timer.WaitForNextTickAsync())
		{
			//Kinda look bulky but two of them is requrired
			await RedisConnection.GetDatabase().StringSetAsync($"server-status:{PlatformRacing3Server.ServerConfig.ServerId}", $"{this.clientManager.Count} online", TimeSpan.FromSeconds(3), When.Always, CommandFlags.FireAndForget);
			await RedisConnection.GetDatabase().PublishAsync("ServerStatusUpdated", $"{PlatformRacing3Server.ServerConfig.ServerId}\0{this.clientManager.Count} online", CommandFlags.FireAndForget);
		}
	}

	public void Shutdown()
	{
		this.listener.Dispose();
	}
        
	public static TimeSpan Uptime => PlatformRacing3Server.StartTime.Elapsed;
}