namespace PlatformRacing3.Server.API.Game.Commands;

public interface ICommandExecutor
{
	uint PermissionRank { get; }

	void SendMessage(string message);

	bool HasPermission(string permission);
}