using PlatformRacing3.Common.Customization;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal class RemoveHatsCommand : ICommand
{
	private readonly CommandManager commandManager;

	public RemoveHatsCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.removehats.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		HashSet<Hat> hats = new HashSet<Hat>();
		for (int i = 1; i < args.Length; i++)
		{
			Hat hat;
			if (uint.TryParse(args[i], out uint hatId))
			{
				hat = (Hat)hatId;
			}
			else if (!Enum.TryParse(args[i], ignoreCase: true, out hat))
			{
				executor.SendMessage($"Unable to find part with name {args[i]}");

				return;
			}

			hats.Add(hat);
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
			if (target is { MultiplayerMatchSession.Match: { } match, MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
			{
				targetsMatched++;

				if (hats.Count == 0)
				{
					match.RemoveHatsFromPlayer(matchPlayer);
				}
				else
				{
					foreach (Hat hat in hats)
					{
						match.RemoveHatFromPlayer(matchPlayer, hat);
					}
				}
			}
		}

		executor.SendMessage($"Effected {targetsMatched} clients");
	}
}
