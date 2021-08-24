using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class RateLevelIncomingMessage : MessageIncomingJson<JsonRateLevelIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonRateLevelIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            LevelManager.RateLevelAsync(message.LevelId, session.UserData.Id, message.Rating);
        }
    }
}
