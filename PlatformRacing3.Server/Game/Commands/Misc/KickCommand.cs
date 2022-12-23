using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Misc;

internal sealed class KickCommand : ICommand
{
	private readonly CommandManager commandManager;

	public KickCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.kick.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length >= 1)
		{
			int i = 0;
			
			foreach (ClientSession target in this.commandManager.GetTargets(executor, args[0]))
			{
				if (target.PermissionRank > executor.PermissionRank)
				{
					continue;
				}

				i++;

				if (args.Length == 1)
				{
					target.Disconnect("You got kicked for absolute no reason, I bet some staff must hate you");
				}
				else
				{
					target.Disconnect(string.Join(' ', args[1..].ToArray()));
				}
			}

			executor.SendMessage($"Effected {i} clients");
		}
		else
		{
			executor.SendMessage("Usage: /kick [user] [reason(empty)]");
		}
	}
}