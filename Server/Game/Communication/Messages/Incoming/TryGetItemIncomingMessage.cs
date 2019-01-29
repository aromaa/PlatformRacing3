using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class TryGetItemIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, IncomingMessage message)
        {
            MultiplayerMatch match = session.MultiplayerMatchSession?.MatchPlayer?.Match;
            if (match != null)
            {
                int x = message.ReadInt();
                int y = message.ReadInt();
                string side = message.ReadString();
                string item = message.ReadString();

                match.TryGetItem(session, x, y, side, item);
            }
        }
    }
}
