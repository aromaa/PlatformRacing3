using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class RateLevelIncomingMessage : MessageIncomingJson<JsonRateLevelIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonRateLevelIncomingMessage message)
	{
		if (session.IsGuest)
		{
			return;
		}

		LevelManager.RateLevelAsync(message.LevelId, session.UserData.Id, message.Rating);
	}
}