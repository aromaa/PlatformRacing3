using System.Net;

namespace PlatformRacing3.Server.Game.User.Identifiers;

internal class PlayerIdentifier : IUserIdentifier
{
	internal uint UserId { get; set; }

	internal PlayerIdentifier(uint userId)
	{
		this.UserId = userId;
	}

	public bool Matches(uint userId, uint socketId, IPAddress ipAddress) => this.UserId == userId;
}