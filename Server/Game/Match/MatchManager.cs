using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Collections;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchManager
    {
        internal ConcurrentDictionary<string, MultiplayerMatch> MultiplayerMatches;

        private volatile int NextMatchId;

        internal MatchManager()
        {
            this.MultiplayerMatches = new ConcurrentDictionary<string, MultiplayerMatch>();
        }

        private uint GetNextMatchId() => (uint)Interlocked.Increment(ref this.NextMatchId);

        internal MultiplayerMatch CreateMultiplayerMatch(MatchListing matchListing)
        {
            MultiplayerMatch match = new MultiplayerMatch("match-" + this.GetNextMatchId(), matchListing.LevelData);
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
    }
}
