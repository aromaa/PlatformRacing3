using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class ThingExistsOutgoingMessage : JsonOutgoingMessage<JsonThingExistsOutgoingMessage>
    {
        internal ThingExistsOutgoingMessage(bool exists) : base(new JsonThingExistsOutgoingMessage(exists))
        {
        }
    }
}
