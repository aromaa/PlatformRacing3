using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class TeleportHereCommand : ICommand
{
	private readonly CommandManager commandManager;

	public TeleportHereCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.teleporthere.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length is < 1 or > 2)
		{
			executor.SendMessage("Usage: /teleporthere [target] <to>");

			return;
		}

		MatchPlayer toMatchPlayer;
		double x;
		double y;
		if (args.Length >= 2)
		{
			using IEnumerator<ClientSession> to = this.commandManager.GetTargets(executor, args[1]).GetEnumerator();
			if (!to.MoveNext())
			{
				executor.SendMessage("To not found");

				return;
			}

			ClientSession toSession = to.Current;
			if (to.MoveNext())
			{
				executor.SendMessage("No single to");

				return;
			}

			if (toSession is not {MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
			{
				executor.SendMessage("Target not in a match");

				return;
			}

			toMatchPlayer = matchPlayer;
			x = matchPlayer.X;
			y = matchPlayer.Y;
		}
		else if (executor is ClientSession { MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
		{
			toMatchPlayer = matchPlayer;
			x = matchPlayer.X;
			y = matchPlayer.Y;
		}
		else
		{
			executor.SendMessage("Target not found");

			return;
		}

		int i = 0;

		foreach (ClientSession whoSession in this.commandManager.GetTargets(executor, args[0]))
		{
			if (whoSession is { MultiplayerMatchSession.MatchPlayer: { } matchPlayer } && matchPlayer.Match == toMatchPlayer.Match)
			{
				i++;

				matchPlayer.X = x;
				matchPlayer.Y = y;

				if (matchPlayer.GetUpdatePacket(out UpdateOutgoingPacket packet))
				{
					matchPlayer.Match.SendPacket(packet);
				}
			}
		}

		executor.SendMessage($"Effected {i} clients");
	}
}
