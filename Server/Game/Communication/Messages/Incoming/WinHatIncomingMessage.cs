using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.User;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
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
                });
            }
            else
            {
                session.UserData.CheckCampaignPrizes(message.Season, message.Medals);
            }
        }
    }
}
