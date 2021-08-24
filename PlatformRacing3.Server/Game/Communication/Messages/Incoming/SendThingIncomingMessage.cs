using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SendThingIncomingMessage : MessageIncomingJson<JsonSendThingIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonSendThingIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            switch(message.Thing)
            {
                case "block":
                    {
                        BlockManager.TransferBlockAsync(message.ThingId, session.UserData.Id, message.ToUserId, message.ThingTitle);
                    }
                    break;
                case "level":
                    {
                        LevelManager.TransferLevelAsync(message.ThingId, session.UserData.Id, message.ToUserId, message.ThingTitle);
                    }
                    break;
            }
        }
    }
}
