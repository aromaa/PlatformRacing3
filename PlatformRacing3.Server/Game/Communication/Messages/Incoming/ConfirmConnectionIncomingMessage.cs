using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class ConfirmConnectionIncomingMessage : IMessageIncomingJson
{
	public void Handle(ClientSession session, JsonPacket message)
	{
		if (session.UpgradeClientStatus(ClientStatus.ConnectionConfirmed))
		{
			session.SendPacket(new VersionOutgoingMessage(PlatformRacing3Server.PROTOCOL_VERSION));
			session.SendPacket(new SocketIdOutgoingMessage(session.SocketId));
		}
	}
}