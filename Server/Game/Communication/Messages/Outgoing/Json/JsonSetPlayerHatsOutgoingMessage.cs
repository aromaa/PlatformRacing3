using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonSetPlayerHatsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "setPlayerHats";

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        [JsonPropertyName("hatArray")]
        public IReadOnlyCollection<MatchPlayerHat> Hats { get; set; }

        internal JsonSetPlayerHatsOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayerHat> hats)
        {
            this.SocketId = socketId;
            this.Hats = hats;
        }
    }
}
