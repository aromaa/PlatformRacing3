using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class DashIncomingMessage : MessageIncomingJson<JsonDashIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonDashIncomingMessage message)
	{
		session.MultiplayerMatchSession?.MatchPlayer?.Match.UpdateDash(session, message.Dash);
	}
}