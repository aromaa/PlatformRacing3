using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.User;

internal sealed class UnmuteCommand : ICommand
{
	private readonly CommandManager commandManager;

	public UnmuteCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}
	
	public string Permission => "command.mute.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length == 1)
		{
			int i = 0;

			foreach (ClientSession target in this.commandManager.GetTargets(executor, args[0]))
			{
				if (target.PermissionRank > executor.PermissionRank || target == executor)
				{
					continue;
				}

				i++;

				target.UserData.Muted = false;

				target.SendMessage("You have been unmuted");
			}

			executor.SendMessage($"Effected {i} clients");
		}
		else
		{
			executor.SendMessage("Usage: /unmute [user]");
		}
	}
}
