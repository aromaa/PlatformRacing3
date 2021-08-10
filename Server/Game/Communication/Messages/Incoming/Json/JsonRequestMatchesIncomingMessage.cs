using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonRequestMatchesIncomingMessage : JsonPacket
    {
        [JsonPropertyName("num")]
        public uint Num { get; set; }

        [JsonPropertyName("lobbyId")]
        public uint LobbyId { get; set; }
    }
}
