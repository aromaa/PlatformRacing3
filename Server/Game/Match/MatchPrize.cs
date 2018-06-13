using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchPrize
    {
        internal string Category { get; }
        internal uint Id { get; }

        internal MatchPrize(string category, uint id)
        {
            this.Category = category;
            this.Id = id;
        }
    }
}
