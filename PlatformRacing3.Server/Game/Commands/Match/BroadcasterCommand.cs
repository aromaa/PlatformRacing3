using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal class BroadcasterCommand : ICommand
{
	public string Permission => "command.broadcaster.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (executor is ClientSession session)
		{
			MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
			if (matchSession != null && matchSession.Match != null)
			{
				matchSession.Match.Broadcaster = !matchSession.Match.Broadcaster;

				if (matchSession.Match.Broadcaster)
				{
					executor.SendMessage("Broadcaster has been signed to duty!");
				}
				else
				{
					executor.SendMessage("Broadcaster has been fired, what a shame!");
				}
			}
			else
			{
				executor.SendMessage("You need to be in match to use this command");
			}
		}
		else
		{
			executor.SendMessage("This command may only be executed by client session");
		}
	}
}