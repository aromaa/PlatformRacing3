using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Extensions
{
    public static class MatchListingTypeExtenssion
    {
        public static string GetLobbyId(this MatchListingType type, uint id)
        {
            switch(type)
            {
                case MatchListingType.Normal:
                    return $"match-listing-{id}";
                case MatchListingType.LevelOfTheDay:
                    return $"lotd-{id}";
                case MatchListingType.Tournament:
                    return $"tournament-{id}";
                default:
                    throw new NotSupportedException(nameof(type));
            }
        }
        public static string GetMatchId(this MatchListingType type, uint id)
        {
            switch (type)
            {
                case MatchListingType.Normal:
                    return $"match-{id}";
                case MatchListingType.LevelOfTheDay:
                    return $"lotd-{id}";
                case MatchListingType.Tournament:
                    return $"tournament-{id}";
                default:
                    throw new NotSupportedException(nameof(type));
            }
        }
    }
}
