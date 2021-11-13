using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages;

internal abstract class MessageIncomingJson<P> : IMessageIncomingJson where P: JsonPacket
{
	public void Handle(ClientSession session, JsonPacket message)
	{
		this.Handle(session, (P)message);
	}

	internal abstract void Handle(ClientSession session, P message);
}