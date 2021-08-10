using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class SpawnAliensOutgoingPacket : JsonOutgoingMessage<JsonSpawnAliensOutgoingPacket>
    {
        internal SpawnAliensOutgoingPacket(uint count, int seed) : base(new JsonSpawnAliensOutgoingPacket(count, seed))
        {
        }
    }
}
