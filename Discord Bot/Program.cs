using Discord.WebSocket;
using System;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;
using System.Reflection;
using Discord_Bot.Core;
using Newtonsoft.Json;
using System.IO;
using Discord_Bot.Config;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Campaign;

namespace Discord_Bot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
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
