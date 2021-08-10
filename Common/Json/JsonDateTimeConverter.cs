using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Json
{
	public sealed class JsonDateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteNumberValue(((DateTimeOffset)value).ToUnixTimeSeconds());
	}
}
