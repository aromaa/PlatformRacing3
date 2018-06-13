using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Json
{
    internal class RawJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>  objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => reader.Value;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteRawValue((string)value);
    }
}
