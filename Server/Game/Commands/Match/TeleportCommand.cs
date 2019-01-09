using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Match;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Match
{
    internal class TeleportCommand : ICommand
    {
        public string Permission => "command.teleport.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                if (args.Length != 2)
                {
                    executor.SendMessage("Usage: /teleport [x] [y]");

                    return;
                }

                if (!double.TryParse(args[0], out double x))
                {
                    executor.SendMessage("The x must be double");

                    return;
                }

                if (!double.TryParse(args[1], out double y))
                {
                    executor.SendMessage("The y must be double");

                    return;
                }

                MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
                if (matchSession != null && matchSession.Match != null && matchSession.MatchPlayer != null)
                {
                    matchSession.MatchPlayer.X += x;
                    matchSession.MatchPlayer.Y -= y;

                    matchSession.Match.SendPacket(matchSession.MatchPlayer.GetUpdatePacket());
                }
                else
                {
                    executor.SendMessage("You need to be in match to use this command");
                }
            }
            else
            {
                executor.SendMessage("This command may only be executed by client session");
            }
        }
    }
}
