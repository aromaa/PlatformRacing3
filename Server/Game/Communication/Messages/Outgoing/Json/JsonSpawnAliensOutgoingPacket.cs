using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonSpawnAliensOutgoingPacket : JsonPacket
    {
        public override string Type => "spawnAliens";

        [JsonPropertyName("count")]
        public uint Id { get; set; }
        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        public JsonSpawnAliensOutgoingPacket(uint id, int seed)
        {
            this.Id = id;
            this.Seed = seed;
        }
    }
}
