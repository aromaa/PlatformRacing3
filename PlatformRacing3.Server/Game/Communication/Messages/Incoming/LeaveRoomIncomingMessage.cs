using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class LeaveRoomIncomingMessage : MessageIncomingJson<JsonLeaveRoomIncomingMessage>
{
	private readonly ChatRoomManager chatRoomManager;

	private readonly MatchListingManager matchListingManager;
	private readonly MatchManager matchManager;

	public LeaveRoomIncomingMessage(ChatRoomManager chatRoomManager, MatchListingManager matchListingManager, MatchManager matchManager)
	{
		this.chatRoomManager = chatRoomManager;

		this.matchListingManager = matchListingManager;
		this.matchManager = matchManager;
	}

	internal override void Handle(ClientSession session, JsonLeaveRoomIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		switch (message.RoomType)
		{
			case "chat":
			{
				this.chatRoomManager.Leave(session, message.RoomName);
			}
				break;
			case "match_listing":
			{
				this.matchListingManager.Leave(session, message.RoomName);
			}
				break;
			case "game":
			{
				this.matchManager.Leave(session, message.RoomName);
			}
				break;
		}
	}
}