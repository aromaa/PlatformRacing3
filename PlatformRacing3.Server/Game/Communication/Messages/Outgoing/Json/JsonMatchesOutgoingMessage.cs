using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
	internal sealed class JsonMatchesOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveMatches";

        [JsonPropertyName("lobbyId")]
        public uint LobbyId { get; set; }

        [JsonPropertyName("matches")]
        public IReadOnlyCollection<IReadOnlyDictionary<string, object>> Matches { get; set; }

        internal JsonMatchesOutgoingMessage(uint lobbyId, IReadOnlyCollection<MatchListing> matchListings)
        {
            this.LobbyId = lobbyId;

            List<IReadOnlyDictionary<string, object>> matches = new();
            foreach(MatchListing matchListing in matchListings)
            {
                matches.Add(matchListing.GetVars("*"));
            }

            this.Matches = matches;
        }
    }
}
