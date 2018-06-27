using Newtonsoft.Json;
using Platform_Racing_3_Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Common.Json
{
    internal class ColorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Color);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => Color.FromArgb(Convert.ToInt32(reader.Value));
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
