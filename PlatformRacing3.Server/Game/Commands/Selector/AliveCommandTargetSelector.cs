﻿using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Selector;

internal sealed class AliveCommandTargetSelector : ICommandTargetSelector
{
	public IEnumerable<ClientSession> FindTargets(ICommandExecutor executor, string parameter)
	{
		if (executor is ClientSession { MultiplayerMatchSession.Match: { } match } client)
		{
			return match.MatchPlayers.Where(p => p.FinishTime is null && !p.Forfiet).Select(p => p.Session);
		}
		else
		{
			return Array.Empty<ClientSession>();
		}
	}
}
