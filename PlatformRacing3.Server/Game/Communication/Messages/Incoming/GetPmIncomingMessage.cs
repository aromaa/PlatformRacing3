using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class GetPmIncomingMessage : MessageIncomingJson<JsonGetPmIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonGetPmIncomingMessage message)
	{
		if (session.IsGuest)
		{
			return;
		}

		IPrivateMessage privateMessage = PrivateMessageManager.GetPrivateMessageAsync(message.MessageId).Result;
		if (privateMessage?.ReceiverId == session.UserData.Id)
		{
			session.SendPacket(new PmOutgoingMessage(privateMessage));
		}
		else
		{
			session.SendPacket(new AlertOutgoingMessage("PM was not found!"));
		}
	}
}