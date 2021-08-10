using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonBeginMatchOutgoingMessage : JsonPacket
    {
        public override string Type => "beginMatch";

        internal JsonBeginMatchOutgoingMessage()
        {

        }
    }
}
