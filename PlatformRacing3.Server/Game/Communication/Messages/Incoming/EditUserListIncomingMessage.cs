using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal class EditUserListIncomingMessage : MessageIncomingJson<JsonEditUserListIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonEditUserListIncomingMessage message)
        {
            switch(message.ListType)
            {
                case "friend":
                    {
                        switch(message.Action)
                        {
                            case "add":
                                {
                                    session.UserData.AddFriend(message.UserId);
                                }
                                break;
                            case "remove":
                                {
                                    session.UserData.RemoveFriend(message.UserId);
                                }
                                break;
                        }
                    }
                    break;
                case "ignored":
                    {
                        switch (message.Action)
                        {
                            case "add":
                                {
                                    session.UserData.AddIgnored(message.UserId);
                                }
                                break;
                            case "remove":
                                {
                                    session.UserData.RemoveIgnored(message.UserId);
                                }
                                break;
                        }
                        break;
                    }
            }
        }
    }
}
