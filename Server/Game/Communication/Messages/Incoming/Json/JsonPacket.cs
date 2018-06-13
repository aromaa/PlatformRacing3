using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonPacket
    {
        [JsonProperty("t")] //Use this as main as it uses less bandwith, thats pretty useaful on long run!
        internal virtual string Type { get; private set; }

        [JsonProperty("type")]
        private string HackyTypeThanksJiggmin //This is needed as some packets use type instead of t, cool...
        {
            set => this.Type = value;
        }

        internal JsonPacket()
        {

        }

        internal JsonPacket(string type)
        {
            this.Type = type;
        }
    }
}
