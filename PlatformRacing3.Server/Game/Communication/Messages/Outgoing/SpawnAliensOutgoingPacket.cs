using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class SpawnAliensOutgoingPacket : JsonOutgoingMessage<JsonSpawnAliensOutgoingPacket>
    {
        internal SpawnAliensOutgoingPacket(uint count, int seed) : base(new JsonSpawnAliensOutgoingPacket(count, seed))
        {
        }
    }
}
