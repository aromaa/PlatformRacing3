using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonMatchOwnerOutgoingMessage : JsonPacket
    {
        internal override string Type => "matchOwner";

        [JsonProperty("matchName")]
        public string Name { get; set; }

        [JsonProperty("play")]
        public bool Play { get; set; }
        [JsonProperty("kick")]
        public bool Kick { get; set; }
        [JsonProperty("ban")]
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
