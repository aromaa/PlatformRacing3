using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class AddHatOutgoingMessage : JsonOutgoingMessage<JsonAddHatOutgoingMessage>
    {
        internal AddHatOutgoingMessage(MatchPlayerHat hat, double x, double y, float velX, float velY) : base(new JsonAddHatOutgoingMessage(hat, x, y, velX, velY))
        {
        }
    }
}
