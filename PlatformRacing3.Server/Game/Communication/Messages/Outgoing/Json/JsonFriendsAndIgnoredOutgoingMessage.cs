using System.Collections.Generic;
using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
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
