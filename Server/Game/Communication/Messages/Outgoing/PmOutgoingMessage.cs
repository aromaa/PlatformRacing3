using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class PmOutgoingMessage : JsonOutgoingMessage<JsonPmOutgoingMessage>
    {
        internal PmOutgoingMessage(IPrivateMessage pm) : base(new JsonPmOutgoingMessage(pm))
        {

        }
    }
}
