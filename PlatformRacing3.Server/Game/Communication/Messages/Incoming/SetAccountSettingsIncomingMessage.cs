using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
	internal class SetAccountSettingsIncomingMessage : MessageIncomingJson<JsonSetAccountSettingsMessage>
    {
        private static readonly HashSet<string> SendResponseVars = new()
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
