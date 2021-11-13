using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Discord.Config;

namespace PlatformRacing3.Discord.Core;

internal class DiscordBot
{
	private DiscordBotConfig Config;

	private CommandService CommandService;

	private DiscordSocketClient Client;

	private IServiceProvider Services;

	internal DiscordBot(DiscordBotConfig config)
	{
		this.Config = config;

		this.CommandService = new CommandService();

		this.Client = new DiscordSocketClient();
		this.Client.MessageReceived += this.HandleIncomingMessage;
	}

	internal async Task LoadCommandsAsync()
	{
		await this.CommandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), this.BuildServeProvider());
	}

	private IServiceProvider BuildServeProvider()
	{
		if (this.Services == null)
		{
			ServerManager serverManager = new(new NullLogger<ServerManager>());
			serverManager.LoadServersAsync().GetAwaiter().GetResult();

			this.Services = new ServiceCollection().AddSingleton(serverManager).BuildServiceProvider();
		}

		return this.Services;
	}

	internal async Task SetupDiscordBot()
	{
		await this.Client.LoginAsync(TokenType.Bot, this.Config.BotToken);
		await this.Client.StartAsync();
	}

	private async Task HandleIncomingMessage(SocketMessage message)
	{
		if (message is SocketUserMessage userMessage)
		{
			if (userMessage.Author.IsBot)
			{
				return;
			}

			int commandIndex = 0;

			if (!userMessage.HasCharPrefix('/', ref commandIndex) && !userMessage.HasMentionPrefix(this.Client.CurrentUser, ref commandIndex))
			{
				return;
			}

			SocketCommandContext context = new(this.Client, userMessage);

			await this.CommandService.ExecuteAsync(context, commandIndex, this.Services);
		}
	}
}