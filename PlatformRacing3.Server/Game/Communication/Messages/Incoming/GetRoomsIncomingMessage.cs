using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class GetRoomsIncomingMessage : IMessageIncomingJson
{
	private readonly ChatRoomManager chatRoomManager;

	public GetRoomsIncomingMessage(ChatRoomManager chatRoomManager)
	{
		this.chatRoomManager = chatRoomManager;
	}

	public void Handle(ClientSession session, JsonPacket message)
	{
		session.SendPacket(new RoomsOutgoingMessage(this.chatRoomManager.Rooms));
	}
}