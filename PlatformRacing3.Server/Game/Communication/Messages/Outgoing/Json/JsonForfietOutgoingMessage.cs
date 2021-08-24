using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonForfietOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveForfiet";

        [JsonPropertyName("socketID")]
        public uint SocketId { get; }

        internal JsonForfietOutgoingMessage(uint socketId)
        {
            this.SocketId = socketId;
        }
    }
}
