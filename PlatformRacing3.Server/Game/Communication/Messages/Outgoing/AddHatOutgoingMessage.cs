using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class AddHatOutgoingMessage : JsonOutgoingMessage<JsonAddHatOutgoingMessage>
{
	internal AddHatOutgoingMessage(MatchPlayerHat hat, double x, double y, float velX, float velY) : base(new JsonAddHatOutgoingMessage(hat, x, y, velX, velY))
	{
	}
}