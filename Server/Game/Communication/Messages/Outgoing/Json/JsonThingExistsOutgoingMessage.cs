using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonThingExistsOutgoingMessage : JsonPacket
    {
        internal override string Type => "thingExits";

        [JsonProperty("exits")]
        internal bool Exists { get; set; }

        internal JsonThingExistsOutgoingMessage(bool exists)
        {
            this.Exists = exists;
        }
    }
}
