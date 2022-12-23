using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class ItemCommand : ICommand
{
	private readonly CommandManager commandManager;

	public ItemCommand(CommandManager commandManager)
	{
		this.commandManager = commandManager;
	}

	public string Permission => "command.item.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length < 1 || args.Length > 2)
		{
			executor.SendMessage("Usage: /item [item] <target>");

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
			if (target is { MultiplayerMatchSession.MatchPlayer: { } matchPlayer })
			{
				i++;

				matchPlayer.Item = args[0];

				if (matchPlayer.GetUpdatePacket(out UpdateOutgoingPacket packet))
				{
					matchPlayer.Match.SendPacket(packet);
				}
			}
		}

		executor.SendMessage($"Effected {i} clients");
	}
}