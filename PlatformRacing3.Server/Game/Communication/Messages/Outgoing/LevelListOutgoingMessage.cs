using System.Collections.Generic;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class LevelListOutgoingMessage : JsonOutgoingMessage<JsonLevelListOutgoingMessage>
    {
        internal LevelListOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<LevelData> levels) : base(new JsonLevelListOutgoingMessage(requestId, results, levels))
        {
        }
    }
}
