using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class AlertOutgoingMessage : JsonOutgoingMessage<JsonAlertOutgoingMessage>
    {
        internal AlertOutgoingMessage(string message) : base(new JsonAlertOutgoingMessage(message))
        {
        }
    }
}
