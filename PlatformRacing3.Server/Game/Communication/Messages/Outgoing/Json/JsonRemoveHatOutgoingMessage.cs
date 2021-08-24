using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonRemoveHatOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "removeHat";

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        internal JsonRemoveHatOutgoingMessage(uint id)
        {
            this.Id = id;
        }
    }
}
