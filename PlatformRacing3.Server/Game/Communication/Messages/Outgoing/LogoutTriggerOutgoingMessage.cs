using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class LogoutTriggerOutgoingMessage : JsonOutgoingMessage<JsonLogoutTriggerOutgoingMessage>
    {
        internal LogoutTriggerOutgoingMessage(string message) : base(new JsonLogoutTriggerOutgoingMessage(message))
        {
        }
    }
}
