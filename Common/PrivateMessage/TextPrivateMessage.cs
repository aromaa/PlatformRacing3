using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Common.PrivateMessage
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TextPrivateMessage : IPrivateMessage
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
        [JsonProperty("message")]
        public string Message { get; }
        [JsonProperty("allowHTML")]
        public bool AllowHtml { get; }
        
        public DateTime SentTime { get; }

        public TextPrivateMessage(uint id, uint receiverId, uint senderId, string senderUsername, Color senderNameColor, string title, string message, bool allowHtml, DateTime sentTime)
        {
            this.Id = id;
            this.ReceiverId = receiverId;

            this.SenderId = senderId;
            this.SenderUsername = senderUsername;
            this.SenderNameColor = senderNameColor;

            this.Title = title;
            this.Message = message;
            this.AllowHtml = allowHtml;

            this.SentTime = sentTime;
        }

        [JsonProperty("senderNameColor")]
        private uint SenderNameColorArgb => (uint)this.SenderNameColor.ToArgb();

        [JsonProperty("sent_time")]
        public ulong SentTimestamp => (ulong)((DateTimeOffset)this.SentTime).ToUnixTimeSeconds();
    }
}
