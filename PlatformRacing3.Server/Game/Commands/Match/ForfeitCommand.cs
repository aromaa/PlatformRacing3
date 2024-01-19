using PlatformRacing3.Common.User;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class ForfeitCommand : ICommand
{
	private readonly CommandManager commandManager;

	public ForfeitCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => null;

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		bool hasPermission = executor.HasPermission("command.forfeit.use");
		if (!hasPermission && executor is not ClientSession { MultiplayerMatchSession: { Match.Status: <= MultiplayerMatchStatus.WaitingForUsersToDraw, MatchPlayer.Host: true } })
		{
			executor.SendMessage("You are lacking the permissions to use this command!");

			return;
		}

		IEnumerable<ClientSession> targets;
		if (args.Length >= 1)
		{
			targets = this.commandManager.GetTargets(executor, args[0]);
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

		int targetsMatched = 0;

		foreach (ClientSession target in targets)
		{
			if (target is { MultiplayerMatchSession.Match: { } match })
			{
				if (target.PermissionRank > executor.PermissionRank && target.HasPermission(Permissions.ACCESS_KICK_IMMUNITY_MATCH_LISTING))
				{
					continue;
				}

				if (!hasPermission && (executor is not ClientSession { MultiplayerMatchSession.Match: { } executorMatch } || match != executorMatch))
				{
					continue;
				}

				targetsMatched++;

				match.Forfeit(target);
			}
		}

		executor.SendMessage($"Effected {targetsMatched} clients");
	}
}
