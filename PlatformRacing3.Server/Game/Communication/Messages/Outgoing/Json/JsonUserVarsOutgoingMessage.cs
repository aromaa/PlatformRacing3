using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonUserVarsOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "receiveUserVars";

	[JsonPropertyName("socketID")]
	public uint SocketId { get; set; }

	[JsonPropertyName("vars")]
	public IReadOnlyDictionary<string, object> Vars { get; set; }

	internal JsonUserVarsOutgoingMessage(uint socketId, IReadOnlyDictionary<string, object> vars)
	{
		this.SocketId = socketId;
		this.Vars = vars;
	}
}