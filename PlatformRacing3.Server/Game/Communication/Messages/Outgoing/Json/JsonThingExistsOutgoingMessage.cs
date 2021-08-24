using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonThingExistsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "thingExits";

        [JsonPropertyName("exits")]
        public bool Exists { get; set; }

        internal JsonThingExistsOutgoingMessage(bool exists)
        {
            this.Exists = exists;
        }
    }
}
