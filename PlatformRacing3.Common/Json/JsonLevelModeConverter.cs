using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Common.Json
{
	public sealed class JsonLevelModeConverter : JsonConverter<LevelMode>
	{
		public override LevelMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

		public override void Write(Utf8JsonWriter writer, LevelMode value, JsonSerializerOptions options)
		{
			string mode = value.ToString();

			writer.WriteStringValue(char.ToLowerInvariant(mode[0]) + mode[1..]);
		}
	}
}
