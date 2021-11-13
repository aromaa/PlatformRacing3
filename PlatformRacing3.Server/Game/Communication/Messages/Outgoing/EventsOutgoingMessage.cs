using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class EventsOutgoingMessage : JsonOutgoingMessage<JsonEventsOutgoingMessage>
{
	internal EventsOutgoingMessage(IReadOnlyCollection<string> events) : base(new JsonEventsOutgoingMessage(events))
	{
	}
}