using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using PlatformRacing3.Discord.Config;

namespace PlatformRacing3.Discord.Core;

internal sealed class DiscordBot
{
	private readonly IServiceProvider serviceProvider;

	private readonly DiscordBotConfig config;
	
	private readonly DiscordSocketClient client;
	private readonly InteractionService interactionService;

	public DiscordBot(IServiceProvider serviceProvider, DiscordBotConfig config)
	{
		this.serviceProvider = serviceProvider;

		this.config = config;
		
		this.client = new DiscordSocketClient();
		this.client.Ready += this.Ready;
		this.client.InteractionCreated += this.InteractionCreated;

		this.interactionService = new InteractionService(this.client);
	}

	internal async Task SetupDiscordBot()
	{
		await this.client.LoginAsync(TokenType.Bot, this.config.BotToken);
		await this.client.StartAsync();
	}

	private async Task Ready()
	{
		await this.interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), this.serviceProvider);
		await this.interactionService.RegisterCommandsGloballyAsync();
	}
	
	private Task InteractionCreated(SocketInteraction interaction)
	{
		SocketInteractionContext context = new(this.client, interaction);
		
		return this.interactionService.ExecuteCommandAsync(context, this.serviceProvider);
	}
}