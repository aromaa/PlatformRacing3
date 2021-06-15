using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonForceMatchOutgoingMessage : JsonPacket
    {
        internal override string Type => "force_match";

        [JsonProperty("roomName")]
        internal string Name { get; set; }

        [JsonProperty("levelID")]
        internal uint LevelId { get; set; }
        [JsonProperty("levelTitle")]
        internal string LevelTitle { get; set; }
        [JsonProperty("version")]
        internal uint LevelVersion { get; set; }

        [JsonProperty("creatorID")]
        internal uint CreatorId { get; set; }
        [JsonProperty("creatorName")]
        internal string CreatorName { get; set; }

        [JsonProperty("levelType")]
        internal string LevelMod { get; set; }

        [JsonProperty("likes")]
        public uint Likes { get; }
        [JsonProperty("dislikes")]
        public uint Dislikes { get; }

        [JsonProperty("minRank")]
        internal uint MinRank { get; }
        [JsonProperty("maxRank")]
        internal uint MaxRank { get; }

        [JsonProperty("maxMembers")]
        internal uint MaxMembers { get; }

        internal JsonForceMatchOutgoingMessage(MatchListing matchListing)
        {
            this.Name = matchListing.Name;

            this.LevelId = matchListing.LevelId;
            this.LevelTitle = matchListing.LevelTitle;
            this.LevelVersion = matchListing.LevelVersion;

            this.CreatorId = matchListing.CreatorId;
            this.CreatorName = matchListing.CreatorName;

            string mode = matchListing.LevelMod.ToString();
            this.LevelMod = Char.ToLowerInvariant(mode[0]) + mode[1..];

            this.Likes = matchListing.Likes;
            this.Dislikes = matchListing.Dislikes;

            this.MinRank = matchListing.MinRank;
            this.MaxRank = matchListing.MaxRank;

            this.MaxMembers = matchListing.MaxMembers;
        }
    }
}
