using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class ShatterBlockOutgoingMessage : JsonOutgoingMessage<JsonShatterBlockMessage>
    {
        internal ShatterBlockOutgoingMessage(string roomName, int tileY, int tileX) : base(new JsonShatterBlockMessage(roomName, tileY, tileX))
        {
        }
    }
}
