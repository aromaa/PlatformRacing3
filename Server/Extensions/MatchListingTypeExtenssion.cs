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
			return type switch
			{
				MatchListingType.Normal => $"match-listing-{id}",
				MatchListingType.LevelOfTheDay => $"lotd-{id}",
				MatchListingType.Tournament => $"tournament-{id}",

				_ => throw new NotSupportedException(nameof(type)),
			};
		}
        public static string GetMatchId(this MatchListingType type, uint id)
        {
			return type switch
			{
				MatchListingType.Normal => $"match-{id}",
				MatchListingType.LevelOfTheDay => $"lotd-{id}",
				MatchListingType.Tournament => $"tournament-{id}",

				_ => throw new NotSupportedException(nameof(type)),
			};
		}
    }
}
