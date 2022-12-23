using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Selector;

internal sealed class MatchCommandTargetSelector : ICommandTargetSelector
{
	public IEnumerable<ClientSession> FindTargets(ICommandExecutor executor, string parameter)
	{
		if (executor is ClientSession { MultiplayerMatchSession.Match: { } match } client)
		{
			return match.Sessions;
		}
		else
		{
			return Array.Empty<ClientSession>();
		}
	}
}
