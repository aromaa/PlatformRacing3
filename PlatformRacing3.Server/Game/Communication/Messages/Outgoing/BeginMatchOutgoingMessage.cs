using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class BeginMatchOutgoingMessage : JsonOutgoingMessage<JsonBeginMatchOutgoingMessage>
    {
        internal BeginMatchOutgoingMessage() : base(new JsonBeginMatchOutgoingMessage())
        {
        }
    }
}
