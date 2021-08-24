using System.Drawing;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class UserPageOutgoingMessage : JsonOutgoingMessage<JsonUserPageOutgoingMessage>
    {
        internal UserPageOutgoingMessage(uint userId, string group, uint rank, bool online, ulong lastLogin, Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor) : base(new JsonUserPageOutgoingMessage(userId, group, rank, online, lastLogin, hat, hatColor, head, headColor, body, bodyColor, feet, feetColor))
        {
        }
    }
}
