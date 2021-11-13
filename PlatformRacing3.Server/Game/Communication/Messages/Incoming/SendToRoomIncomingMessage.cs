using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class SendToRoomIncomingMessage : MessageIncomingJson<JsonSendToRoomIncomingMessage>
{
	private readonly ChatRoomManager chatRoomManager;

	public SendToRoomIncomingMessage(ChatRoomManager chatRoomManager)
	{
		this.chatRoomManager = chatRoomManager;
	}

	internal override void Handle(ClientSession session, JsonSendToRoomIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		switch (message.RoomType)
		{
			case "chat":
			{
				if (session.MultiplayerMatchSession?.Match.Name != message.RoomName)
				{
					if (this.chatRoomManager.TryGet(message.RoomName, out ChatRoom chatRoom))
					{
						chatRoom.HandleData(session, message.Data, message.SendToSelf);
					}
				}
				else
				{
					session.MultiplayerMatchSession?.Match.HandleData(session, message.Data, message.SendToSelf);
				}
			}
				break;
			case "game":
			{
				MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
				if (matchPlayer != null && matchPlayer.Match.Name == message.RoomName)
				{
					matchPlayer.Match.HandleData(session, message.Data, message.SendToSelf);
				}
			}
				break;
		}
	}
}