using Discord.Webhook;
using System.Text.Json.Serialization;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Config;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Commands;
using Platform_Racing_3_Server.Game.Communication.Handlers;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server.Game.Lobby;
using Platform_Racing_3_Server.Game.Match;
using Platform_Racing_3_Server_API.Core;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Sockets.Listener;
using Platform_Racing_3_Common.Utils;

namespace Platform_Racing_3_Server.Core
{
    internal sealed class PlatformRacing3Server : PlatformRacing3
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

        private Timer statusTimer;

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

            PlatformRacing3Server.ServerConfig = JsonSerializer.Deserialize<ServerConfig>(File.ReadAllText("settings.json"));

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

            this.statusTimer = new Timer(this.UpdateStatus, null, 1000, 1000);
        }

        private void UpdateStatus(object state)
        {
            //Kinda look bulky but two of them is requrired
            RedisConnection.GetDatabase().StringSetAsync($"server-status:{PlatformRacing3Server.ServerConfig.ServerId}", $"{this.clientManager.Count} online", TimeSpan.FromSeconds(3), When.Always, CommandFlags.FireAndForget);
            RedisConnection.GetDatabase().PublishAsync("ServerStatusUpdated", $"{PlatformRacing3Server.ServerConfig.ServerId}\0{this.clientManager.Count} online");
        }

        public void Shutdown()
        {
            this.listener.Dispose();

            this.statusTimer.Dispose();
        }
        
        public static TimeSpan Uptime => PlatformRacing3Server.StartTime.Elapsed;
    }
}
