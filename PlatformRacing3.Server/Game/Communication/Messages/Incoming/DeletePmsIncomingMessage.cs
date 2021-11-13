using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class DeletePmsIncomingMessage : MessageIncomingJson<JsonDeletePmsIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonDeletePmsIncomingMessage message)
	{
		if (session.IsGuest)
		{
			return;
		}

		PrivateMessageManager.DeletePMsAsync(message.PMs, session.UserData.Id).Wait();
	}
}