using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonFinishDrawingOutgoingMessage : JsonPacket
    {
        internal override string Type => "finishDrawing";

        [JsonProperty("socketID")]
        internal uint SocketId { get; set; }

        internal JsonFinishDrawingOutgoingMessage(uint socketId)
        {
            this.SocketId = socketId;
        }
    }
}
