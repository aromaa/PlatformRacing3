using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
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
