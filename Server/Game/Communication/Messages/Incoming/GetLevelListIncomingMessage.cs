using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetLevelListIncomingMessage : MessageIncomingJson<JsonGetLevelListIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetLevelListIncomingMessage message)
        {
            switch (message.Mode)
            {
                case "campaign":
                    {
                        (uint results, IReadOnlyCollection<LevelData> levels) = LevelManager.GetCampaignLevels(message.Data, message.Start, message.Count).Result;

                        session.SendPacket(new LevelListOutgoingMessage(message.RequestId, results, levels));
                    }
                    break;
                case "best":
                    {
                        (uint results, IReadOnlyCollection<LevelData> levels) = LevelManager.GetBestLevels(message.Start, message.Count, session.UserData).Result;

                        session.SendPacket(new LevelListOutgoingMessage(message.RequestId, results, levels));
                    }
                    break;
                case "best_today":
                    {
                        (uint results, IReadOnlyCollection<LevelData> levels) = LevelManager.GetBestTodayLevels(message.Start, message.Count, session.UserData).Result;

                        session.SendPacket(new LevelListOutgoingMessage(message.RequestId, results, levels));
                    }
                    break;
                case "newest":
                    {
                        (uint results, IReadOnlyCollection<LevelData> levels) = LevelManager.GetNewestLevels(message.Start, message.Count, session.UserData).Result;

                        session.SendPacket(new LevelListOutgoingMessage(message.RequestId, results, levels));
                    }
                    break;
            }
        }
    }
}
