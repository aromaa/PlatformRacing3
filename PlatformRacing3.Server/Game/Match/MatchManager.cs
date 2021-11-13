using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using PlatformRacing3.Server.Extensions;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Commands;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Match
{
	internal sealed class MatchManager
    {
        private readonly CommandManager commandManager;

        private readonly ILoggerFactory loggerFactory;

        internal ConcurrentDictionary<string, MultiplayerMatch> MultiplayerMatches;

        private volatile int NextMatchId;

        public MatchManager(CommandManager commandManager, ILoggerFactory loggerFactory)
        {
            this.commandManager = commandManager;

            this.loggerFactory = loggerFactory;

            this.MultiplayerMatches = new ConcurrentDictionary<string, MultiplayerMatch>();
        }

        private uint GetNextMatchId() => (uint)Interlocked.Increment(ref this.NextMatchId);

        internal MultiplayerMatch CreateMultiplayerMatch(MatchListing matchListing)
        {
            MultiplayerMatch match = new(this, this.commandManager, this.loggerFactory.CreateLogger<MultiplayerMatch>(), matchListing.Type, matchListing.Type.GetMatchId(this.GetNextMatchId()), matchListing.LevelData);
            if (this.MultiplayerMatches.TryAdd(match.Name, match))
            {
                return match;
            }

            return null;
        }

        internal void JoinMultiplayerMatch(ClientSession session, string roomName)
        {
            if (this.MultiplayerMatches.TryGetValue(roomName, out MultiplayerMatch match))
            {
                match.Join(session);
            }
        }

        internal void Leave(ClientSession session, string roomName)
        {
            if (this.MultiplayerMatches.TryGetValue(roomName, out MultiplayerMatch match))
            {
                match.Leave(session);
            }
        }

        internal void Die(MultiplayerMatch match)
        {
            this.MultiplayerMatches.TryRemove(match.Name, out _);
        }

        internal bool HasOngoingTournaments => this.MultiplayerMatches.Values.FirstOrDefault((m) => m.Type == MatchListingType.Tournament && m.Status != MultiplayerMatchStatus.Ended && m.Status != MultiplayerMatchStatus.Died) != null;
    }
}
