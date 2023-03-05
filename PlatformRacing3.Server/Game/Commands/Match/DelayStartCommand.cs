using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal class DelayStartCommand : ICommand
{
	public string Permission => "command.delaystart.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (executor is ClientSession { MultiplayerMatchSession.Match: { } match })
		{
			if (match.Status <= MultiplayerMatchStatus.WaitingForUsersToDraw)
			{
				match.DelayedStart = !match.DelayedStart;
				match.CheckGameState();

				executor.SendMessage(match.DelayedStart ? "Delayed start enabled" : "Delayed start disabled");
			}
			else
			{
				executor.SendMessage("The game has started!");
			}
		}
		else if (executor is ClientSession { LobbySession.MatchListing: { } matchListing })
		{
			matchListing.DelayedStart = !matchListing.DelayedStart;

			executor.SendMessage(matchListing.DelayedStart ? "Delayed start enabled" : "Delayed start disabled");
		}
		else
		{
			executor.SendMessage("You are not in a match!");
		}
	}
}
