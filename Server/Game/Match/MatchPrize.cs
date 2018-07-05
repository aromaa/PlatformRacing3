using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchPrize
    {
        internal string Category { get; }
        internal uint Id { get; }
        internal bool RewardsExpBonus { get; }

        internal MatchPrize(string category, uint id, bool rewardsExpBonus = true)
        {
            this.Category = category;
            this.Id = id;
            this.RewardsExpBonus = rewardsExpBonus;
        }
    }
}
