using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonBanFromMatchListingIncomingMessage : JsonPacket
    {
        [JsonPropertyName("socket_id")]
        public uint SocketId { get; set; }
    }
}
