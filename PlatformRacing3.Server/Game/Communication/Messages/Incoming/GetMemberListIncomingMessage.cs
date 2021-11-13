using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class GetMemberListIncomingMessage : MessageIncomingJson<JsonGetMemberListIncomingMessage>
{
	private readonly ChatRoomManager chatRoomManager;

	public GetMemberListIncomingMessage(ChatRoomManager chatRoomManager)
	{
		this.chatRoomManager = chatRoomManager;
	}

	internal override void Handle(ClientSession session, JsonGetMemberListIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		if (this.chatRoomManager.TryGet(message.RoomName, out ChatRoom chatRoom))
		{
			session.SendPacket(new MemberListOutgoingMessage(chatRoom.Members));
		}
	}
}