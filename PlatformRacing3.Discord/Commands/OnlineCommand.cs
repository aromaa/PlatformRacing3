using System.Text;
using Discord.Interactions;
using PlatformRacing3.Common.Server;

namespace PlatformRacing3.Discord.Commands;

public sealed class OnlineCommand : InteractionModuleBase<SocketInteractionContext>
{
	private ServerManager serverManager;

	public OnlineCommand(ServerManager serverManager)
	{
		this.serverManager = serverManager;
	}

	[SlashCommand("pr3online", "Shows the online player count.")]
	public Task GetOnlinePlayersCountCommand()
	{
		StringBuilder stringBuilder = new();

		foreach (ServerDetails server in this.serverManager.GetServers())
		{
			stringBuilder.Append(server.Name);
			stringBuilder.Append(": ");
			stringBuilder.Append(server.Status);
			stringBuilder.AppendLine();
		}

		return this.RespondAsync(stringBuilder.ToString());
	}
}