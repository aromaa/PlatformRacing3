using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters
{
    internal class JsonColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Color);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int argb = serializer.Deserialize<int>(reader);

            return Color.FromArgb(argb);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
