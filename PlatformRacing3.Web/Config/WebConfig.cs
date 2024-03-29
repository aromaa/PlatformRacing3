﻿using System.Text.Json.Serialization;
using PlatformRacing3.Common.Config;

namespace PlatformRacing3.Web.Config;

public class WebConfig : IDatabaseConfig, IRedisConfig, IEmailConfig
{
	[JsonPropertyName("database_host")]
	public string DatabaseHost { get; set; }
	[JsonPropertyName("database_port")]
	public uint DatabasePort { get; set; }

	[JsonPropertyName("database_user")]
	public string DatabaseUser { get; set; }
	[JsonPropertyName("database_pass")]
	public string DatabasePass { get; set; }
	[JsonPropertyName("database_name")]
	public string DatabaseName { get; set; }

	[JsonPropertyName("redis_host")]
	public string RedisHost { get; set; }
	[JsonPropertyName("redis_port")]
	public uint RedisPort { get; set; }
        
	[JsonPropertyName("smtp_host")]
	public string SmtpHost { get; set; }
	[JsonPropertyName("smtp_port")]
	public ushort SmtpPort { get; set; }

	[JsonPropertyName("smtp_user")]
	public string SmtpUser { get; set; }
	[JsonPropertyName("smtp_pass")]
	public string SmtpPass { get; set; }
	[JsonPropertyName("smtp_from")]
	public string SmtpFrom { get; set; }

	[JsonPropertyName("discord_client_id")]
	public string DiscordClientId { get; set; }
	[JsonPropertyName("discord_client_secret")]
	public string DiscordClientSecret { get; set; }

	[JsonPropertyName("game_path")]
	public string GamePath { get; set; }
}