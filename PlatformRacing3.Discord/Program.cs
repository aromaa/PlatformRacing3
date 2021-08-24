using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Discord.Config;
using PlatformRacing3.Discord.Core;

namespace PlatformRacing3.Discord
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            LoggerUtil.LoggerFactory = NullLoggerFactory.Instance;

            DiscordBotConfig config = JsonConvert.DeserializeObject<DiscordBotConfig>(File.ReadAllText("settings.json"));

            DatabaseConnection.Init(config);
            RedisConnection.Init(config);

            CampaignManager campaignManager = new();

            Task.WaitAll(campaignManager.LoadCampaignTimesAsync(), campaignManager.LoadPrizesAsync());

            DiscordBot bot = new(config);

            await bot.LoadCommandsAsync();
            await bot.SetupDiscordBot();

            Console.WriteLine("Ready!");

            await Task.Delay(-1);
        }
    }
}
