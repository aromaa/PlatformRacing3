using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonBanFromMatchListingIncomingMessage : JsonPacket
    {
        [JsonProperty("socket_id", Required = Required.Always)]
        internal uint SocketId { get; set; }
    }
}
