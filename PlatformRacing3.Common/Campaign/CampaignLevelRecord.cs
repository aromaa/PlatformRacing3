using System.Text.Json.Serialization;

namespace PlatformRacing3.Common.Campaign
{
    public struct CampaignLevelRecord
    {
        [JsonPropertyName("timeMS")]
        public int Time { get; }

        [JsonPropertyName("season")]
        public string Season { get; }

        [JsonPropertyName("medal")]
        public CampaignMedal Medal { get; }

        public CampaignLevelRecord(int time, string season, CampaignMedal medal)
        {
            this.Time = time;
            this.Season = season;
            this.Medal = medal;
        }
    }
}
