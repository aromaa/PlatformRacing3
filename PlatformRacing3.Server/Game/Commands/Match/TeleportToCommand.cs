using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class TeleportToCommand : ICommand
{
	private readonly CommandManager commandManager;

	public TeleportToCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.teleportto.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length is < 1 or > 2)
		{
			executor.SendMessage("Usage: /teleportto [target] <who>");

			return;
		}

		using IEnumerator<ClientSession> targets = this.commandManager.GetTargets(executor, args[0]).GetEnumerator();
		if (!targets.MoveNext())
		{
			executor.SendMessage("Target not found");

			return;
		}

		ClientSession target = targets.Current;
		if (targets.MoveNext())
		{
			executor.SendMessage("No single target");

			return;
		}

		if (target is not { MultiplayerMatchSession.MatchPlayer: { } targetMatchPlayer })
		{
			executor.SendMessage("Target not in a match");

			return;
		}

		IEnumerable<ClientSession> who;
		if (args.Length >= 2)
		{
			who = this.commandManager.GetTargets(executor, args[1]);
		}
		else if (executor is ClientSession session)
		{
			who = new ClientSession[] { session };
		}
		else
		{
			executor.SendMessage("No who");

			return;
		}

		double x = targetMatchPlayer.X;
		double y = targetMatchPlayer.Y;

		int i = 0;

		foreach (ClientSession whoSession in who)
		{
			if (whoSession is { MultiplayerMatchSession.MatchPlayer: { } matchPlayer } && matchPlayer.Match == targetMatchPlayer.Match)
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
