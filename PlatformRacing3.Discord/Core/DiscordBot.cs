using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using PlatformRacing3.Common.User;
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
		this.client.ReactionAdded += this.ReactionAdded;

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

	private async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
	{
		if (channel.Id != 1062483837000626206)
		{
			return;
		}

		if (reaction.UserId != 131910893603782657)
		{
			return;
		}

		if (reaction.Emote.Name != "\u2764\uFE0F")
		{
			return;
		}

		IUserMessage cachedMessage = await message.GetOrDownloadAsync();

		uint userId = await UserManager.HasDiscordLinkage(cachedMessage.Author.Id);
		if (userId == 0)
		{
			return;
		}

		PlayerUserData userData = await UserManager.TryGetUserDataByIdAsync(userId);

		IUser reactionUser = reaction.User.IsSpecified
			? reaction.User.Value
			: await this.client.GetUserAsync(reaction.UserId);

		IDMChannel dmChannel = await reactionUser.CreateDMChannelAsync();

		await dmChannel.SendMessageAsync("You have given 3 bonus exp to " + userData.Username);

		userData.GiveBonusExp(3);
	}
}
