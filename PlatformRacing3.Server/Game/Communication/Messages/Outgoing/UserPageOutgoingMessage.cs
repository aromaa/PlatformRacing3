using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class UserPageOutgoingMessage : JsonOutgoingMessage<JsonUserPageOutgoingMessage>
    {
        internal UserPageOutgoingMessage(uint userId, string group, uint rank, bool online, ulong lastLogin, Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor) : base(new JsonUserPageOutgoingMessage(userId, group, rank, online, lastLogin, hat, hatColor, head, headColor, body, bodyColor, feet, feetColor))
        {
        }
    }
}
