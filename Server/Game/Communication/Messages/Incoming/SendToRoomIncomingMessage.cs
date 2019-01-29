using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SendToRoomIncomingMessage : MessageIncomingJson<JsonSendToRoomIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonSendToRoomIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            switch (message.RoomType)
            {
                case "chat":
                    {
                        if (session.MultiplayerMatchSession?.Match.Name != message.RoomName)
                        {
                            if (PlatformRacing3Server.ChatRoomManager.TryGet(message.RoomName, out ChatRoom chatRoom))
                            {
                                chatRoom.HandleData(session, message.Data, message.SendToSelf);
                            }
                        }
                        else
                        {
                            session.MultiplayerMatchSession?.Match.HandleData(session, message.Data, message.SendToSelf);
                        }
                    }
                    break;
                case "game":
                    {
                        MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
                        if (matchPlayer != null && matchPlayer.Match.Name == message.RoomName)
                        {
                            matchPlayer.Match.HandleData(session, message.Data, message.SendToSelf);
                        }
                    }
                    break;
            }
        }
    }
}
