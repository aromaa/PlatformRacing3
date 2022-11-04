using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Match;

internal sealed class SpectateCommand : ICommand
{
	public string Permission => "command.spectate.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (executor is ClientSession session)
		{
			session.Spectate = true;

			executor.SendMessage("The next match will be joined as a spectator");
		}
		else
		{
			executor.SendMessage("This command may only be executed by client session");
		}
	}
}
