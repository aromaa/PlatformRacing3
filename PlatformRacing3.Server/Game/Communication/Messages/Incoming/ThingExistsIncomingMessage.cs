using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class ThingExistsIncomingMessage : MessageIncomingJson<JsonThingExistsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonThingExistsIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            switch(message.ThingType)
            {
                case "block":
                    {
                        BlockData blockData = BlockManager.GetBlockAsync(session.UserData.Id, message.ThingTitle, message.ThingCategory).Result;

                        session.SendPacket(new ThingExistsOutgoingMessage(blockData != null));
                    }
                    break;
                case "level":
                    {
                        LevelData levelData = LevelManager.GetLevelDataAsync(session.UserData.Id, message.ThingTitle).Result;

                        session.SendPacket(new ThingExistsOutgoingMessage(levelData != null));
                    }
                    break;
            }
        }
    }
}
