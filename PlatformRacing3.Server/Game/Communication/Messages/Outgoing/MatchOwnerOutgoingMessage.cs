using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class MatchOwnerOutgoingMessage : JsonOutgoingMessage<JsonMatchOwnerOutgoingMessage>
{
	internal MatchOwnerOutgoingMessage(string name, bool play, bool kick, bool ban) : base(new JsonMatchOwnerOutgoingMessage(name, play, kick, ban))
	{
	}
}