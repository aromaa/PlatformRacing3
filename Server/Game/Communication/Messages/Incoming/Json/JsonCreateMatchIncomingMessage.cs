using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonCreateMatchIncomingMessage : JsonPacket
    {
        [JsonProperty("level_id", Required = Required.Always)]
        public uint LevelId { get; set; }

        [JsonProperty("version", Required = Required.Always)]
        public uint Version { get; set; }

        [JsonProperty("min_rank", Required = Required.Always)]
        public uint MinRank { get; set; }

        [JsonProperty("max_rank", Required = Required.Always)]
        public uint MaxRank { get; set; }

        [JsonProperty("max_members", Required = Required.Always)]
        public uint MaxMembers { get; set; }

        [JsonProperty("only_friends", Required = Required.Always)]
        public bool OnlyFriends { get; set; }
    }
}
