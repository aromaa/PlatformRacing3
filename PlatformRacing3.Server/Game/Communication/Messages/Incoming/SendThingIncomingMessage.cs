using PlatformRacing3.Common.Block;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
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
