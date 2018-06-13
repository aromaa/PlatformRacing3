using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Converters;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class PmsOutgoingMessage : JsonOutgoingMessage
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings();

        static PmsOutgoingMessage()
        {
            PmsOutgoingMessage.JsonSerializerSettings.ContractResolver = new JsonCherryPickSerializableFieldsContractResolver(new Dictionary<Type, HashSet<string>>()
            {
                {
                    typeof(IPrivateMessage), new HashSet<string>()
                    {
                        "messageID",
                        "senderId",
                        "senderName",
                        "senderNameColor",
                        "title",
                        "sent_time"
                    }
                }
            });
        }

        internal PmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms) : base(new JsonPmsOutgoingMessage(requestId, results, pms))
        {
        }

        protected override string SerializeObject(JsonPacket jsonPacket) => JsonConvert.SerializeObject(jsonPacket, PmsOutgoingMessage.JsonSerializerSettings);
    }
}
