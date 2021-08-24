using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class ThingExistsOutgoingMessage : JsonOutgoingMessage<JsonThingExistsOutgoingMessage>
    {
        internal ThingExistsOutgoingMessage(bool exists) : base(new JsonThingExistsOutgoingMessage(exists))
        {
        }
    }
}
