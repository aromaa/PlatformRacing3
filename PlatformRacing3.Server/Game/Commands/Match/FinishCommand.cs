using PlatformRacing3.Common.User;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class FinishCommand : ICommand
{
	private readonly CommandManager commandManager;

	public FinishCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.finish.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
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
				if (target.PermissionRank > executor.PermissionRank)
				{
					continue;
				}

				targetsMatched++;

				match.FinishMatch(target);
			}
		}

		executor.SendMessage($"Effected {targetsMatched} clients");
	}
}
