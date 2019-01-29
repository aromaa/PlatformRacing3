using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MultiplayerMatchSession
    {
        internal MultiplayerMatch Match { get; }
        internal MatchPlayer MatchPlayer { get; private set; }

        internal MultiplayerMatchSession(MultiplayerMatch match, MatchPlayer player)
        {
            this.Match = match;
            this.MatchPlayer = player;
        }

        internal void Forfiet()
        {
            this.MatchPlayer = null;
        }
    }
}
