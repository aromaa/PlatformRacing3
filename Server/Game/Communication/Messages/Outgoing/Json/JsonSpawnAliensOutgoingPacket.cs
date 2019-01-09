using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonSpawnAliensOutgoingPacket : JsonPacket
    {
        internal override string Type => "spawnAliens";

        [JsonProperty("count")]
        internal uint Id { get; set; }
        [JsonProperty("seed")]
        internal int Seed { get; set; }

        public JsonSpawnAliensOutgoingPacket(uint id, int seed)
        {
            this.Id = id;
            this.Seed = seed;
        }
    }
}
