using PlatformRacing3.Common.User;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class WinHatIncomingMessage : MessageIncomingJson<JsonWinHatIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonWinHatIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		if (session.UserData is PlayerUserData playerUserData)
		{
			UserManager.GetCampaignRuns(session.UserData.Id).ContinueWith((r) =>
			{
				foreach (KeyValuePair<uint, int> run in r.Result)
				{
					playerUserData.CheckCampaignTime(run.Key, run.Value);
				}

				session.UserData.CheckCampaignPrizes(message.Season, message.Medals);

				session.SendPacket(new UserVarsOutgoingMessage(session.SocketId, session.UserData.GetVars("hatArray", "headArray", "bodyArray", "feetArray")));
			});
		}
		else
		{
			session.UserData.CheckCampaignPrizes(message.Season, message.Medals);
		}
	}
}