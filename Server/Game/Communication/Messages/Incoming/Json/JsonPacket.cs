using System.Text.Json.Serialization;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal abstract class JsonPacket
    {
        [JsonPropertyName("t")] //Only use the "t" property as the packet type indicator when serializing because we use less bandwidth
        public string Type => this.InternalType;

	    private protected virtual string InternalType { get; }

        private protected JsonPacket()
        {
        }
    }
}
