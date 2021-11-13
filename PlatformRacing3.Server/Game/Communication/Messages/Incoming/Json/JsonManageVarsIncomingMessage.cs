using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
	internal sealed class JsonManageVarsIncomingMessage : JsonPacket
    {
        [JsonPropertyName("user_vars")]
        public HashSet<string> UserVars { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
