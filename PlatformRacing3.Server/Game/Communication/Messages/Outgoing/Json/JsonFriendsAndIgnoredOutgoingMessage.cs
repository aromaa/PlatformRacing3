using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonFriendsAndIgnoredOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveFriendsAndIgnored";

        [JsonPropertyName("friendArray")]
        public IReadOnlyCollection<uint> Friends { get; set; }

        [JsonPropertyName("ignoredArray")]
        public IReadOnlyCollection<uint> Ignored { get; set; }

        internal JsonFriendsAndIgnoredOutgoingMessage(IReadOnlyCollection<uint> friends, IReadOnlyCollection<uint> ignored)
        {
            this.Friends = friends;
            this.Ignored = ignored;
        }
    }
}
