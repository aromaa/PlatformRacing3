using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonSocketIdOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveSocketID";

        [JsonProperty("socketID")]
        internal uint SocketID { get; set; }

        internal JsonSocketIdOutgoingMessage(uint socketId)
        {
            this.SocketID = socketId;
        }
    }
}
