using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Commands.Misc;

internal sealed class AlertCommand : ICommand
{
	private readonly CommandManager commandManager;

	public AlertCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.alert.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length >= 2)
		{
			int i = 0;

			foreach (ClientSession target in this.commandManager.GetTargets(executor, args[0]))
			{
				i++;
				
				target.SendPacket(new AlertOutgoingMessage(string.Join(' ', args[1..].ToArray())));
			}

			executor.SendMessage($"Effected {i} clients");
		}
		else
		{
			executor.SendMessage("Usage: /alert [user] [message]");
		}
	}
}