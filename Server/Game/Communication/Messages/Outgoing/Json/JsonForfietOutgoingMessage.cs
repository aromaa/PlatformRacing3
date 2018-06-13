using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonForfietOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveForfiet";

        [JsonProperty("socketID")]
        internal uint SocketId { get; }

        internal JsonForfietOutgoingMessage(uint socketId)
        {
            this.SocketId = socketId;
        }
    }
}
