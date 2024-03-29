﻿using System.Text.Json.Serialization;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonLevelListOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "receiveLevelList";

	[JsonPropertyName("requestID")]
	public uint RequestId { get; set; }

	[JsonPropertyName("levels")]
	public IReadOnlyCollection<LevelData> Levels { get; set; }

	[JsonPropertyName("results")]
	public uint Results { get; set; }

	internal JsonLevelListOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<LevelData> levels)
	{
		this.RequestId = requestId;
		this.Results = results;
		this.Levels = levels;
	}
}