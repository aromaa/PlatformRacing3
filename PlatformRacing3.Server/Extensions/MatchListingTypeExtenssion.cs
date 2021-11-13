using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Extensions
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
