using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
	internal class ManageVarsIncomingMessage : MessageIncomingJson<JsonManageVarsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonManageVarsIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            switch (message.Location)
            {
                case "user":
                    {
                        switch(message.Action)
                        {
                            case "get":
                                {
                                    if (uint.TryParse(message.Id, out uint socketId))
                                    {
                                        if (session.SocketId == socketId)
                                        {
                                            session.SendPacket(new UserVarsOutgoingMessage(session.SocketId, session.UserData.GetVars(message.UserVars)));
                                        }
                                        else
                                        {
                                            throw new Exception("You may only request your own user vars");
                                        }
                                    }
                                    else
                                    {
                                        throw new FormatException(nameof(message.Id));
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
