using System.Net;

namespace PlatformRacing3.Server.Game.User.Identifiers;

public interface IUserIdentifier
{
	bool Matches(uint userId, uint socketId, IPAddress ipAddress);
}