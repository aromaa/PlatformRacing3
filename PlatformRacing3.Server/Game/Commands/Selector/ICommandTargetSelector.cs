using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Selector;

internal interface ICommandTargetSelector
{
	public IEnumerable<ClientSession> FindTargets(ICommandExecutor executor, string parameter);
}
