using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Converters
{
    internal class JsonCherryPickSerializableFieldsContractResolver : DefaultContractResolver
    {
        private Dictionary<Type, HashSet<string>> CherryPickByType;

        internal JsonCherryPickSerializableFieldsContractResolver(Dictionary<Type, HashSet<string>> cherryPick)
        {
            this.CherryPickByType = cherryPick;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            foreach (KeyValuePair<Type, HashSet<string>> cherryPick in this.CherryPickByType)
            {
                if (cherryPick.Key.IsAssignableFrom(member.DeclaringType))
                {
                    JsonPropertyAttribute jsonPropertyAttribute = member.GetCustomAttribute<JsonPropertyAttribute>();
                    if (jsonPropertyAttribute == null || !cherryPick.Value.Contains(jsonPropertyAttribute.PropertyName))
                    {
                        return null;
                    }
                }
            }

            return base.CreateProperty(member, memberSerialization);
        }
    }
}
