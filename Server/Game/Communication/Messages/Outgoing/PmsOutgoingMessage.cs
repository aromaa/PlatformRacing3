using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
	internal class PmsOutgoingMessage : JsonOutgoingMessage<JsonPmsOutgoingMessage>
	{
		internal PmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms) : base(new JsonPmsOutgoingMessage(requestId, results, pms))
		{
		}
	}
}
