using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonMatchOwnerOutgoingMessage : JsonPacket
    {
        public override string Type => "matchOwner";

        [JsonPropertyName("matchName")]
        public string Name { get; set; }

        [JsonPropertyName("play")]
        public bool Play { get; set; }
        [JsonPropertyName("kick")]
        public bool Kick { get; set; }
        [JsonPropertyName("ban")]
        public bool Ban { get; set; }

        internal JsonMatchOwnerOutgoingMessage(string name, bool play, bool kick, bool ban)
        {
            this.Name = name;

            this.Play = play;
            this.Kick = kick;
            this.Ban = ban;
        }
    }
}
