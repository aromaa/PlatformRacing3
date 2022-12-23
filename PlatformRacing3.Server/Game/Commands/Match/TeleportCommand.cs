using PlatformRacing3.Common.Customization;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class TeleportCommand : ICommand
{
	private readonly CommandManager commandManager;

	public TeleportCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.teleport.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length < 2 || args.Length > 3)
		{
			executor.SendMessage("Usage: /teleport [x] [y] <target>");

			return;
		}

		if (!double.TryParse(args[0], out double x))
		{
			executor.SendMessage("The x must be double");

			return;
		}

		if (!double.TryParse(args[1], out double y))
		{
			executor.SendMessage("The y must be double");

			return;
		}
		
		IEnumerable<ClientSession> targets;
		if (args.Length >= 3)
		{
			targets = this.commandManager.GetTargets(executor, args[2]);
		}
		else if (executor is ClientSession session)
		{
			targets = new ClientSession[] { session };
		}
		else
		{
			executor.SendMessage("No target");

			return;
		}

		int i = 0;

		foreach (ClientSession target in targets)
		{
			if (target is { MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
			{
				i++;

				matchPlayer.X += x * 40;
				matchPlayer.Y -= y * 40;

				if (matchPlayer.GetUpdatePacket(out UpdateOutgoingPacket packet))
				{
					matchPlayer.Match.SendPacket(packet);
				}
			}
		}

		executor.SendMessage($"Effected {i} clients");
	}
}