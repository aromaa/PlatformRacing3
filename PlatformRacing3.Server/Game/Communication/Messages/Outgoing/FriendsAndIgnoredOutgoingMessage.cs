using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class FriendsAndIgnoredOutgoingMessage : JsonOutgoingMessage<JsonFriendsAndIgnoredOutgoingMessage>
{
	internal FriendsAndIgnoredOutgoingMessage(IReadOnlyCollection<uint> friends, IReadOnlyCollection<uint> ignored) : base(new JsonFriendsAndIgnoredOutgoingMessage(friends, ignored))
	{
	}
}