using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Common.PrivateMessage;

public class ThingTransferPrivateMessage : IPrivateMessage
{
	public uint Id { get; }
	public uint ReceiverId { get; }
        
	public uint SenderId { get; }
	public string SenderUsername { get; }
	public Color SenderNameColor { get; }
        
	public string Title { get; }
	public string Message { get; }
        
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
		this.Message = JsonSerializer.Serialize(new ThingTransferData(senderUsername, thingType, thingTitle, id));

		this.ThingType = thingType;

		this.SentTime = sentTime;
	}

	private sealed class ThingTransferData
	{
		[JsonPropertyName("message_type")]
		public string ThingReceive => "thing_receive";

		[JsonPropertyName("thing_sender")]
		public string ThingSender { get; set; }

		[JsonPropertyName("thing_type")]
		public string ThingType { get; set; }

		[JsonPropertyName("thing_title")]
		public string ThingTitle { get; set; }

		[JsonPropertyName("thing_id")]
		public uint ThingId { get; set; }

		internal ThingTransferData(string thingSender, string thingType, string thingTitle, uint thingId)
		{
			this.ThingSender = thingSender;
			this.ThingType = thingType;
			this.ThingTitle = thingTitle;
			this.ThingId = thingId;
		}
	}
}