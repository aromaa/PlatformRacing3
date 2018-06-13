using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonFriendsAndIgnoredOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveFriendsAndIgnored";

        [JsonProperty("friendArray")]
        internal IReadOnlyCollection<uint> Friends { get; set; }

        [JsonProperty("ignoredArray")]
        internal IReadOnlyCollection<uint> Ignored { get; set; }

        internal JsonFriendsAndIgnoredOutgoingMessage(IReadOnlyCollection<uint> friends, IReadOnlyCollection<uint> ignored)
        {
            this.Friends = friends;
            this.Ignored = ignored;
        }
    }
}
