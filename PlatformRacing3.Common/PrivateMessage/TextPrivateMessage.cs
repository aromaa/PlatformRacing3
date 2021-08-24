using System;
using System.Drawing;

namespace PlatformRacing3.Common.PrivateMessage
{
    public class TextPrivateMessage : IPrivateMessage
    {
        public uint Id { get; }
        public uint ReceiverId { get; }
        
        public uint SenderId { get; }
        public string SenderUsername { get; }
        public Color SenderNameColor { get; }
        
        public string Title { get; }
        public string Message { get; }
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
    }
}
