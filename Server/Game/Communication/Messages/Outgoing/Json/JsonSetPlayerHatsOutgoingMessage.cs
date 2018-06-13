using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonSetPlayerHatsOutgoingMessage : JsonPacket
    {
        internal override string Type => "setPlayerHats";

        [JsonProperty("socketID")]
        internal uint SocketId { get; set; }

        [JsonProperty("hatArray")]
        internal IReadOnlyCollection<MatchPlayerHat> Hats { get; set; }

        internal JsonSetPlayerHatsOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayerHat> hats)
        {
            this.SocketId = socketId;
            this.Hats = hats;
        }
    }
}
