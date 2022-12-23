using PlatformRacing3.Common.Customization;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal class AddHatCommand : ICommand
{
	private readonly CommandManager commandManager;

	public AddHatCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.addhat.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length < 1 || args.Length > 2)
		{
			executor.SendMessage("Usage: /addhat [hat] <target>");

			return;
		}

		Hat hat;
		if (uint.TryParse(args[0], out uint hatId))
		{
			hat = (Hat)hatId;
		}
		else if (!Enum.TryParse(args[0], ignoreCase: true, out hat))
		{
			executor.SendMessage($"Unable to find part with name {args[0]}");

			return;
		}
			
		IEnumerable<ClientSession> targets;
		if (args.Length >= 2)
		{
			targets = this.commandManager.GetTargets(executor, args[1]);
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
			if (target is { MultiplayerMatchSession.Match: { } match, MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
			{
				i++;
					
				match.AddHatToPlayer(matchPlayer, hat, matchPlayer.UserData.CurrentHatColor, spawned: true);
			}
		}

		executor.SendMessage($"Effected {i} clients");
	}
}