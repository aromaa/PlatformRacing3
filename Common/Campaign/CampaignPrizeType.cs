using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Campaign
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
