using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Common.PrivateMessage
{
    public interface IPrivateMessage
    {
	    [JsonPropertyName("messageID")]
        uint Id { get; }
        [JsonIgnore]
        uint ReceiverId { get; }

        [JsonPropertyName("senderId")]
        uint SenderId { get; }
        [JsonPropertyName("senderName")]
        string SenderUsername { get; }
        [JsonPropertyName("senderNameColor")]
        Color SenderNameColor { get; }

        [JsonPropertyName("title")]
        string Title { get; }
        [JsonPropertyName("message")]
        string Message { get; }

        [JsonPropertyName("sent_time")]
        DateTime SentTime { get; }
    }
}
