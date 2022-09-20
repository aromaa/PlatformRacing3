using Discord;
using Discord.Interactions;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Discord.Commands;

public sealed class VerifyCommand : InteractionModuleBase<SocketInteractionContext>
{
	[SlashCommand("pr3verify", "Verify your account.")]
	public async Task VerifyAccounCommand()
	{
		uint userId = await UserManager.HasDiscordLinkage(this.Context.User.Id);
		if (userId == 0)
		{
			await this.RespondAsync("Link your Discord account with PR3 one here: https://api.pr3hub.com/linkdiscord");
		}
		else
		{
			IRole role = this.Context.Guild.GetRole(595041143776083988);

			await ((IGuildUser)this.Context.User).AddRoleAsync(role);
			await this.RespondAsync(":sunglasses:");
		}
	}
}