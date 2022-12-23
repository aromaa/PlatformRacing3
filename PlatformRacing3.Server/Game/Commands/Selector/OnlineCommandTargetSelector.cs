using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Selector;

internal sealed class OnlineCommandTargetSelector : ICommandTargetSelector
{
	private readonly ClientManager clientManager;

	internal OnlineCommandTargetSelector(ClientManager clientManager)
	{
		this.clientManager = clientManager;
	}

	public IEnumerable<ClientSession> FindTargets(ICommandExecutor executor, string parameter) => this.clientManager.LoggedInUsers;
}
