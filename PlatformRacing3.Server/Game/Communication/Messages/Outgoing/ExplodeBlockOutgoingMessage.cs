using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class ExplodeBlockOutgoingMessage : JsonOutgoingMessage<JsonExplodeBlockMessage>
    {
        internal ExplodeBlockOutgoingMessage(string roomName, int tileY, int tileX) : base(new JsonExplodeBlockMessage(roomName, tileY, tileX))
        {
        }
    }
}
