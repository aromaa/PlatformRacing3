﻿using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonKothIncomingMessage : JsonPacket
{
	[JsonPropertyName("time")]
	public required string Time { get; init; }
}