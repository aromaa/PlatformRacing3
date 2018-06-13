using Newtonsoft.Json;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonUserPageOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveUserPage";

        [JsonProperty("userID")]
        internal uint UserId { get; set; }
        [JsonProperty("group")]
        internal string Group { get; set; }
        [JsonProperty("rank")]
        internal uint Rank { get; set; }

        [JsonProperty("online")]
        internal bool Online { get; set; }
        [JsonProperty("timestamp")]
        internal ulong Timestamp { get; set; }

        [JsonProperty("hat")]
        internal Hat Hat { get; set; }
        [JsonProperty("hatColor")]
        internal uint HatColor { get; set; }

        [JsonProperty("head")]
        internal Part Head { get; set; }
        [JsonProperty("headColor")]
        internal uint HeadColor { get; set; }

        [JsonProperty("body")]
        internal Part Body { get; set; }
        [JsonProperty("bodyColor")]
        internal uint BodyColor { get; set; }

        [JsonProperty("feet")]
        internal Part Feet { get; set; }
        [JsonProperty("feetColor")]
        internal uint FeetColor { get; set; }

        internal JsonUserPageOutgoingMessage(uint userId, string group, uint rank, bool online, ulong timestamp, Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor)
        {
            this.UserId = userId;
            this.Group = group;
            this.Rank = rank;

            this.Online = online;
            this.Timestamp = timestamp;

            this.Hat = hat;
            this.HatColor = (uint)hatColor.ToArgb();

            this.Head = head;
            this.HeadColor = (uint)headColor.ToArgb();

            this.Body = body;
            this.BodyColor = (uint)bodyColor.ToArgb();

            this.Feet = feet;
            this.FeetColor = (uint)feetColor.ToArgb();
        }
    }
}
