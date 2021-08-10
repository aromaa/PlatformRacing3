using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonForceMatchOutgoingMessage : JsonPacket
    {
        public override string Type => "force_match";

        [JsonPropertyName("roomName")]
        public string Name { get; set; }

        [JsonPropertyName("levelID")]
        public uint LevelId { get; set; }
        [JsonPropertyName("levelTitle")]
        public string LevelTitle { get; set; }
        [JsonPropertyName("version")]
        public uint LevelVersion { get; set; }

        [JsonPropertyName("creatorID")]
        public uint CreatorId { get; set; }
        [JsonPropertyName("creatorName")]
        public string CreatorName { get; set; }

        [JsonPropertyName("levelType")]
        public string LevelMod { get; set; }

        [JsonPropertyName("likes")]
        public uint Likes { get; }
        [JsonPropertyName("dislikes")]
        public uint Dislikes { get; }

        [JsonPropertyName("minRank")]
        public uint MinRank { get; }
        [JsonPropertyName("maxRank")]
        public uint MaxRank { get; }

        [JsonPropertyName("maxMembers")]
        public uint MaxMembers { get; }

        internal JsonForceMatchOutgoingMessage(MatchListing matchListing)
        {
            this.Name = matchListing.Name;

            this.LevelId = matchListing.LevelId;
            this.LevelTitle = matchListing.LevelTitle;
            this.LevelVersion = matchListing.LevelVersion;

            this.CreatorId = matchListing.CreatorId;
            this.CreatorName = matchListing.CreatorName;

            string mode = matchListing.LevelMod.ToString();
            this.LevelMod = char.ToLowerInvariant(mode[0]) + mode[1..];

            this.Likes = matchListing.Likes;
            this.Dislikes = matchListing.Dislikes;

            this.MinRank = matchListing.MinRank;
            this.MaxRank = matchListing.MaxRank;

            this.MaxMembers = matchListing.MaxMembers;
        }
    }
}
