using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Campaign
{
    public struct CampaignLevelRecord
    {
        [JsonProperty("timeMS")]
        public int Time { get; }

        [JsonProperty("season")]
        public string Season { get; }

        [JsonProperty("medal")]
        public CampaignMedal Medal { get; }

        public CampaignLevelRecord(int time, string season, CampaignMedal medal)
        {
            this.Time = time;
            this.Season = season;
            this.Medal = medal;
        }
    }
}
