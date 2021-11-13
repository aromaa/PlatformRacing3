using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class GetPmsIncomingMessage : MessageIncomingJson<JsonGetPmsIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonGetPmsIncomingMessage message)
	{
		if (session.IsGuest)
		{
			return;
		}

		PrivateMessageManager.GetUserPMsAsync(session.UserData.Id, message.Start, message.Count).ContinueWith((task) =>
		{
			(uint Results, IReadOnlyList<IPrivateMessage> PMs) = task.Result;

			session.SendPacket(new PmsOutgoingMessage(message.RequestId, Results, PMs));
		});
	}
}