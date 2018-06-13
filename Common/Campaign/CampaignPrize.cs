using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Platform_Racing_3_Common.Campaign
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CampaignPrize
    {
        [JsonProperty("id")]
        public uint Id { get; }
        public CampaignPrizeType Type { get; }

        [JsonProperty("medals")]
        public uint MedalsRequired { get; }

        internal CampaignPrize(DbDataReader reader)
        {
            this.Id = (uint)(int)reader["id"];
            this.Type = (CampaignPrizeType)reader["type"];
            this.MedalsRequired = (uint)(int)reader["medals_required"];
        }

        [JsonProperty("category")]
        private string Category => this.Type.ToString().ToLowerInvariant();
    }
}
