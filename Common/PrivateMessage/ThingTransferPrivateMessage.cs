using Newtonsoft.Json;
using Platform_Racing_3_Common.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Common.PrivateMessage
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ThingTransferPrivateMessage : IPrivateMessage
    {
        [JsonProperty("messageID")]
        public uint Id { get; }
        public uint ReceiverId { get; }

        [JsonProperty("senderId")]
        public uint SenderId { get; }
        [JsonProperty("senderName")]
        public string SenderUsername { get; }
        public Color SenderNameColor { get; }

        [JsonProperty("title")]
        public string Title { get; }
        [JsonConverter(typeof(RawJsonConverter))]
        [JsonProperty("message")]
        public string Message { get; }
        [JsonProperty("handleAsJson")]
        public bool HandleAsJson => true;

        public string ThingType { get; }

        public DateTime SentTime { get; }

        public ThingTransferPrivateMessage(uint id, uint receiverId, uint senderId, string senderUsername, Color senderNameColor, string thingType, string thingTitle, uint thingId, DateTime sentTime)
        {
            this.Id = id;
            this.ReceiverId = receiverId;

            this.SenderId = senderId;
            this.SenderUsername = senderUsername;
            this.SenderNameColor = senderNameColor;

            this.Title = $"{senderUsername} has sent you a {thingType}";
            this.Message = JsonConvert.SerializeObject(new ThingTransferData(senderUsername, thingType, thingTitle, id));

            this.ThingType = thingType;

            this.SentTime = sentTime;
        }

        [JsonProperty("senderNameColor")]
        private uint SenderNameColorArgb => (uint)this.SenderNameColor.ToArgb();

        [JsonProperty("sent_time")]
        public ulong SentTimestamp => (ulong)((DateTimeOffset)this.SentTime).ToUnixTimeSeconds();

        private class ThingTransferData
        {
            [JsonProperty("message_type")]
            private string ThingReceive => "thing_receive";

            [JsonProperty("thing_sender")]
            internal string ThingSender { get; set; }

            [JsonProperty("thing_type")]
            internal string ThingType { get; set; }

            [JsonProperty("thing_title")]
            internal string ThingTitle { get; set; }

            [JsonProperty("thing_id")]
            internal uint ThingId { get; set; }

            internal ThingTransferData(string thingSender, string thingType, string thingTitle, uint thingId)
            {
                this.ThingSender = thingSender;
                this.ThingType = thingType;
                this.ThingTitle = thingTitle;
                this.ThingId = thingId;
            }
        }
    }
}
