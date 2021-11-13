using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetMemberListIncomingMessage : JsonPacket
{
	[JsonPropertyName("room_name")]
	public string RoomName { get; set; }
}