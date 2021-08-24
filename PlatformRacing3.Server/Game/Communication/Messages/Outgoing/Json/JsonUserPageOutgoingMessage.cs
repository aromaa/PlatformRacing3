using System.Text.Json.Serialization;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserPageOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveUserPage";

        [JsonPropertyName("userID")]
        public uint UserId { get; set; }
        [JsonPropertyName("group")]
        public string Group { get; set; }
        [JsonPropertyName("rank")]
        public uint Rank { get; set; }

        [JsonPropertyName("online")]
        public bool Online { get; set; }
        [JsonPropertyName("timestamp")]
        public ulong Timestamp { get; set; }

        [JsonPropertyName("hat")]
        public Hat Hat { get; set; }
        [JsonPropertyName("hatColor")]
        public uint HatColor { get; set; }

        [JsonPropertyName("head")]
        public Part Head { get; set; }
        [JsonPropertyName("headColor")]
        public uint HeadColor { get; set; }

        [JsonPropertyName("body")]
        public Part Body { get; set; }
        [JsonPropertyName("bodyColor")]
        public uint BodyColor { get; set; }

        [JsonPropertyName("feet")]
        public Part Feet { get; set; }
        [JsonPropertyName("feetColor")]
        public uint FeetColor { get; set; }

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
