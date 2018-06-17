using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Lobby
{
    public enum MatchListingJoinStatus
    {
        Failed,
        Banned,
        NoRankRequirement,
        FriendsOnly,
        Full,
        Started,
        Died,
        Success,
    }
}
