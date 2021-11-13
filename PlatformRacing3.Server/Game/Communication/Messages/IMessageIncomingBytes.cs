using Net.Buffers;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Communication.Messages;

internal interface IMessageIncomingBytes
{
	void Handle(ClientSession session, ref PacketReader reader);
}