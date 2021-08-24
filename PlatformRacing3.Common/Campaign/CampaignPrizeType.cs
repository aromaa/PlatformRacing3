using NpgsqlTypes;

namespace PlatformRacing3.Common.Campaign
{
    public enum CampaignPrizeType : uint
    {
        [PgName("hat")]
        Hat,
        [PgName("head")]
        Head,
        [PgName("body")]
        Body,
        [PgName("feet")]
        Feet,
    }
}
