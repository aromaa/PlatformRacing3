using System.Text.Json.Serialization;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal abstract class JsonPacket
    {
        [JsonPropertyName("t")] //Only use the "t" property as the packet type indicator when serializing because we use less bandwidth
        public virtual string Type { get; init; }

        [JsonPropertyName("type")] //For whatever reason, the client uses the "type" property instead of the "t" in some cases to indicate which packet it is, only use this for deserializing
        public string T
        {
            init => this.Type = value;
        }

        private protected JsonPacket()
        {
        }
    }
}
