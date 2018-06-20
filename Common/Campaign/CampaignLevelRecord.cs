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

        [JsonProperty("medal")]
        public CampaignMedal Medal { get; }

        public CampaignLevelRecord(int time, CampaignMedal medal)
        {
            this.Time = time;
            this.Medal = medal;
        }
    }
}
