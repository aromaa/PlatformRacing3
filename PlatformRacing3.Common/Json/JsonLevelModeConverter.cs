using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Platform_Racing_3_Common.Level;

namespace Platform_Racing_3_Common.Json
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
