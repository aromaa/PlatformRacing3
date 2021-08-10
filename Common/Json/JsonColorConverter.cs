﻿using Platform_Racing_3_Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Common.Json
{
    public sealed class JsonColorConverter : JsonConverter<Color>
    {
		public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Color.FromArgb(reader.GetInt32());
		public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) => writer.WriteNumberValue(value.ToArgb());
	}
}