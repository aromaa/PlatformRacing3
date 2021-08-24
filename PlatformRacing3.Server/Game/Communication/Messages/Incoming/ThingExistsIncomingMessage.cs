using PlatformRacing3.Common.Block;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
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
