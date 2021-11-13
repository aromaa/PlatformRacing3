using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal class SpawnAliensCommand : ICommand
{
	public string Permission => "command.spawnaliens.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (executor is ClientSession session)
		{
			if (args.Length != 1)
			{
				executor.SendMessage("Usage: /spawnaliens [amount]");

				return;
			}

			if (!uint.TryParse(args[0], out uint amount))
			{
				executor.SendMessage("The amount must be unsigned integer");

				return;
			}

			MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
			if (matchSession != null && matchSession.Match != null)
			{
				matchSession.Match.SendPacket(new SpawnAliensOutgoingPacket(amount, new Random().Next(26487)));
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