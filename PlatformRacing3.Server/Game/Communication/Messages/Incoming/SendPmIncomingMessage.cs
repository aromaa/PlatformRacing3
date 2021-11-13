using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Common.User;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class SendPmIncomingMessage : MessageIncomingJson<JsonSendPmIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonSendPmIncomingMessage message)
	{
		if (session.IsGuest)
		{
			return;
		}

		if (message.Message.Length > 0)
		{
			UserManager.TryGetUserDataByNameAsync(message.ReceiverUsername).ContinueWith((task) =>
			{
				PlayerUserData userData = task.Result;
				if (userData != null)
				{
					PrivateMessageManager.SendTextPrivateMessageAsync(userData.Id, session.UserData.Id, message.Title, message.Message);
				}
				else
				{
					session.SendPacket(new AlertOutgoingMessage("User was not found!"));
				}
			});
		}
		else
		{
			session.SendPacket(new AlertOutgoingMessage("What if you typed message before sending?"));
		}
	}
}