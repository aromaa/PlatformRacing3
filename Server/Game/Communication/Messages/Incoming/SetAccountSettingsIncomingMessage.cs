using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SetAccountSettingsIncomingMessage : MessageIncomingJson<JsonSetAccountSettingsMessage>
    {
        private static readonly HashSet<string> SendResponseVars = new HashSet<string>()
        {
            "hat",
            "hatColor",

            "head",
            "headColor",

            "body",
            "bodyColor",

            "feet",
            "feetColor",

            "speed",
            "accel",
            "jump",
        };

        internal override void Handle(ClientSession session, JsonSetAccountSettingsMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.UserData.SetStats(message.Speed, message.Accel, message.Jump);
            session.UserData.SetParts(message.Hat, message.HatColor, message.Head, message.HeadColor, message.Body, message.BodyColor, message.Feet, message.FeetColor);
            
            session.SendPacket(new UserVarsOutgoingMessage(session.SocketId, session.UserData.GetVars(SetAccountSettingsIncomingMessage.SendResponseVars)));
        }
    }
}
