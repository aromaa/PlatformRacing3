using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Selector;

internal sealed class MeCommandTargetSelector : ICommandTargetSelector
{
	public IEnumerable<ClientSession> FindTargets(ICommandExecutor executor, string parameter)
	{
		if (executor is ClientSession client)
		{
			return new ClientSession[] { client };
		}
		else
		{
			return Array.Empty<ClientSession>();
		}
	}
}
