using Newtonsoft.Json;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonMatchCreatedOutgoingMessage : JsonPacket
    {
        internal override string Type => "matchCreated";

        [JsonProperty("roomName")]
        internal string Name { get; set; }

        [JsonProperty("levelID")]
        internal uint LevelId { get; set; }
        [JsonProperty("levelTitle")]
        internal string LevelTitle { get; set; }

        [JsonProperty("creatorID")]
        internal uint CreatorId { get; set; }
        [JsonProperty("creatorName")]
        internal string CreatorName { get; set; }
        [JsonProperty("creatorNameColor")]
        internal uint CreatorNameColor { get; set; }

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

        internal JsonMatchCreatedOutgoingMessage(MatchListing matchListing)
        {
            this.Name = matchListing.Name;

            this.LevelId = matchListing.LevelId;
            this.LevelTitle = matchListing.LevelTitle;

            this.CreatorId = matchListing.CreatorId;
            this.CreatorName = matchListing.CreatorName;
            this.CreatorNameColor = (uint)matchListing.CreatorNameColor.ToArgb();

            string mode = matchListing.LevelMod.ToString();
            this.LevelMod = Char.ToLowerInvariant(mode[0]) + mode.Substring(1);

            this.Likes = matchListing.Likes;
            this.Dislikes = matchListing.Dislikes;

            this.MinRank = matchListing.MinRank;
            this.MaxRank = matchListing.MaxRank;

            this.MaxMembers = matchListing.MaxMembers;
        }
    }
}
