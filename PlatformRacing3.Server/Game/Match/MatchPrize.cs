using PlatformRacing3.Common.Customization;

namespace PlatformRacing3.Server.Game.Match
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

        public override string ToString()
        {
            //Very ugly! Yikes, when interfaces?
            return $"{(this.Category == "hat" ? ((Hat)this.Id).ToString() : ((Part)this.Id).ToString())} {this.Category}";
        }
    }
}
