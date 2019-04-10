using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonMatchesOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveMatches";

        [JsonProperty("lobbyId")]
        public uint LobbyId { get; set; }

        [JsonProperty("matches")]
        public IReadOnlyCollection<IReadOnlyDictionary<string, object>> Matches { get; set; }

        internal JsonMatchesOutgoingMessage(uint lobbyId, IReadOnlyCollection<MatchListing> matchListings)
        {
            this.LobbyId = lobbyId;

            List<IReadOnlyDictionary<string, object>> matches = new List<IReadOnlyDictionary<string, object>>();
            foreach(MatchListing matchListing in matchListings)
            {
                matches.Add(matchListing.GetVars("*"));
            }

            this.Matches = matches;
        }
    }
}
