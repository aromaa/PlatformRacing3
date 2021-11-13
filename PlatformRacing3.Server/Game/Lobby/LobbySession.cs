using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Lobby
{
	internal class LobbySession
    {
        private ClientSession Session { get; }

        private List<MatchListing> _Matches { get; }
        internal MatchListing MatchListing { get; set; }

        internal LobbySession(ClientSession session)
        {
            this._Matches = new List<MatchListing>(4);

            this.Session = session;
        }

        internal IReadOnlyCollection<MatchListing> Matches => this._Matches.AsReadOnly();

        internal void AddMatches(List<MatchListing> listings)
        {
            foreach(MatchListing listing in listings)
            {
                this.AddMatch(listing);
            }
        }

        internal void AddMatch(MatchListing listing)
        {
            if (listing.JoinLobby(this.Session))
            {
                this._Matches.Add(listing);
            }
        }

        internal void RemoveMatch(MatchListing listing)
        {
            this._Matches.Remove(listing);

            listing.LeaveLobby(this.Session);
        }

        internal void LeaveLobby()
        {
            foreach(MatchListing listing in this._Matches.ToList())
            {
                listing.LeaveLobby(this.Session);
            }

            this._Matches.Clear();
        }
    }
}
