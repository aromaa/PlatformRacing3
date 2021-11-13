using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Misc;

internal sealed class KickCommand : ICommand
{
	private readonly ClientManager clientManager;

	public KickCommand(ClientManager clientManager)
	{
		this.clientManager = clientManager;
	}

	public string Permission => "command.kick.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length >= 1)
		{
			ClientSession target = this.clientManager.GetClientSessionByUsername(args[0]);
			if (target != null)
			{
				if (target.PermissionRank > executor.PermissionRank)
				{
					executor.SendMessage("You do not have permissions to kick this user");

					return;
				}

				if (args.Length == 1)
				{
					target.Disconnect("You got kicked for absolute no reason, I bet some staff must hate you");
				}
				else
				{
					target.Disconnect(string.Join(' ', args[1..].ToArray()));
				}
			}
			else
			{
				executor.SendMessage($"Unable to find user online named {args[0]}");
			}
		}
		else
		{
			executor.SendMessage("Usage: /kick [user] [reason(empty)]");
		}
	}
}