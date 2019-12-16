using System;
using System.Collections.Generic;
using System.Text;
using Net.Communication.Incoming.Helpers;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class TryGetItemIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, ref PacketReader reader)
        {
            MultiplayerMatch match = session.MultiplayerMatchSession?.MatchPlayer?.Match;
            if (match != null)
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                string side = reader.ReadFixedString();
                string item = reader.ReadFixedString();

                match.TryGetItem(session, x, y, side, item);
            }
        }
    }
}
