using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.User;

internal sealed class MuteCommand : ICommand
{
	private readonly CommandManager commandManager;

	public MuteCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}
	
	public string Permission => "command.mute.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length >= 1)
		{
			int i = 0;

			foreach (ClientSession target in this.commandManager.GetTargets(executor, args[0]))
			{
				if (target.PermissionRank > executor.PermissionRank || target == executor)
				{
					continue;
				}

				i++;

				target.UserData.Muted = true;

				if (args.Length == 1)
				{
					target.SendMessage("You have been muted for absolute no reason, I bet some staff must hate you");
				}
				else
				{
					target.SendMessage("You have been muted for " + string.Join(' ', args[1..].ToArray()));
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
