using Discord.Webhook;
using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Net.Sockets.Listener;
using Ninject;

namespace Platform_Racing_3_Server.Core
{
    internal sealed class PlatformRacing3Server : PlatformRacing3
    {
        public const uint PROTOCOL_VERSION = 24;
        
        private static Stopwatch StartTime { get; set; }

        public static ServerConfig ServerConfig { get; set; }

        public static ServerManager ServerManager { get; private set; }
        public static ChatRoomManager ChatRoomManager { get; private set; }

        public static CampaignManager CampaignManager { get; private set; }

        public static BytePacketManager BytePacketManager { get; private set; }
        public static PacketManager PacketManager { get; private set; }
        public static ClientManager ClientManager { get; private set; }
        public static IListener Listener { get; private set; }

        public static DiscordWebhookClient DiscordChatWebhook { get; private set; }
        public static DiscordWebhookClient DiscordNotificationsWebhook { get; private set; }

        public static Timer StatusTimer { get; private set; }

        public PlatformRacing3Server(PacketManager packetManager, ChatRoomManager chatRoomManager)
        {
            PlatformRacing3Server.PacketManager = packetManager;
            PlatformRacing3Server.ChatRoomManager = chatRoomManager;
        }

        internal void Init()
        {
            PlatformRacing3Server.StartTime = Stopwatch.StartNew();

            PlatformRacing3Server.ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText("settings.json"));

            RedisConnection.Init(PlatformRacing3Server.ServerConfig);
            DatabaseConnection.Init(PlatformRacing3Server.ServerConfig);

            PlatformRacing3Server.ServerManager = new ServerManager();
            PlatformRacing3Server.ServerManager.LoadServersAsync().Wait();

            PlatformRacing3Server.CampaignManager = new CampaignManager();
            PlatformRacing3Server.CampaignManager.LoadCampaignTimesAsync().Wait();
            PlatformRacing3Server.CampaignManager.LoadPrizesAsync().Wait();

            PlatformRacing3Server.BytePacketManager = new BytePacketManager(new StandardKernel());
            PlatformRacing3Server.ClientManager = new ClientManager();
            PlatformRacing3Server.Listener = IListener.CreateTcpListener(new IPEndPoint(IPAddress.Parse(PlatformRacing3Server.ServerConfig.BindIp), PlatformRacing3Server.ServerConfig.BindPort), socket =>
            {
                socket.Pipeline.AddHandlerFirst(new SplitPacketHandler(new ClientSession(socket)));
                socket.Pipeline.AddHandlerFirst(new FlashSocketPolicyRequestHandler());
            });

            if (PlatformRacing3Server.ServerConfig.DiscordChatWebhookId != 0)
            {
                PlatformRacing3Server.DiscordChatWebhook = new DiscordWebhookClient(PlatformRacing3Server.ServerConfig.DiscordChatWebhookId, PlatformRacing3Server.ServerConfig.DiscordChatWebhookToken);
            }

            if (PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookId != 0)
            {
                PlatformRacing3Server.DiscordNotificationsWebhook = new DiscordWebhookClient(PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookId, PlatformRacing3Server.ServerConfig.DiscordNotificationsWebhookToken);
            }

            PlatformRacing3Server.StatusTimer = new Timer(this.UpdateStatus, null, 1000, 1000);
        }

        private void UpdateStatus(object state)
        {
            //Kinda look bulky but two of them is requrired
            RedisConnection.GetDatabase().StringSetAsync($"server-status:{PlatformRacing3Server.ServerConfig.ServerId}", $"{PlatformRacing3Server.ClientManager.Count} online", TimeSpan.FromSeconds(3), When.Always, CommandFlags.FireAndForget);
            RedisConnection.GetDatabase().PublishAsync("ServerStatusUpdated", $"{PlatformRacing3Server.ServerConfig.ServerId}\0{PlatformRacing3Server.ClientManager.Count} online");
        }

        public void Shutdown()
        {
            PlatformRacing3Server.Listener.Dispose();
        }
        
        public static TimeSpan Uptime => PlatformRacing3Server.StartTime.Elapsed;
    }
}
