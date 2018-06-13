using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonLoseHatIncomingMessage : JsonPacket
    {
        [JsonProperty("x", Required = Required.Always)]
        internal double X { get; set; }
        [JsonProperty("y", Required = Required.Always)]
        internal double Y {get; set;}

        [JsonProperty("vel_x", Required = Required.Always)]
        internal float VelX { get; set; }
        [JsonProperty("vel_y", Required = Required.Always)]
        internal float VelY { get; set; }
    }
}
