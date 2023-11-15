using Discord.Interactions;

namespace PlatformRacing3.Discord.Commands;

public sealed class VerifyCommand : InteractionModuleBase<SocketInteractionContext>
{
	[SlashCommand("pr3verify", "Verify your account.")]
	public async Task VerifyAccountCommand()
	{
		await this.RespondAsync("Click server name (Banner, top left, above channels) -> Linked roles -> Verified");
	}
}