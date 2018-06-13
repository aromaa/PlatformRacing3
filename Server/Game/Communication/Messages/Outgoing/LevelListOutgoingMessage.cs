using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class LevelListOutgoingMessage : JsonOutgoingMessage
    {
        internal LevelListOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<LevelData> levels) : base(new JsonLevelListOutgoingMessage(requestId, results, levels))
        {
        }
    }
}
