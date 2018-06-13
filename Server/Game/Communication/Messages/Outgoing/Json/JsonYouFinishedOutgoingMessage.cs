using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonYouFinishedOutgoingMessage : JsonPacket
    {
        internal override string Type => "youFinished";

        [JsonProperty("rank")]
        internal ulong Rank { get; set; }

        [JsonProperty("curExp")]
        internal ulong CurExp { get; set; }

        [JsonProperty("maxExp")]
        internal ulong MaxExp { get; set; }

        [JsonProperty("totExpGain")]
        internal ulong TotExpGain { get; set; }

        [JsonProperty("expArray")]
        internal IReadOnlyCollection<object[]> ExpArray { get; set; }

        internal JsonYouFinishedOutgoingMessage(uint rank, ulong curExp, ulong maxExp, ulong totExpGain, IReadOnlyCollection<object[]> expArray)
        {
            this.Rank = rank;
            this.CurExp = curExp;
            this.MaxExp = maxExp;
            this.TotExpGain = totExpGain;
            this.ExpArray = expArray;
        }
    }
}
