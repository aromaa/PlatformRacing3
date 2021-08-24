using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class UseItemOutgoingMessage : JsonOutgoingMessage<JsonUseItemMessage>
    {
        internal UseItemOutgoingMessage(string roomName, uint socketId, double[] pos) : base(new JsonUseItemMessage(roomName, socketId, pos))
        {
        }
    }
}
