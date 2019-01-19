using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Match;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Match
{
    internal class BroadcasterCommand : ICommand
    {
        public string Permission => "command.broadcaster.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
                if (matchSession != null && matchSession.Match != null && matchSession.MatchPlayer != null)
                {
                    matchSession.Match.Broadcaster = !matchSession.Match.Broadcaster;

                    if (matchSession.Match.Broadcaster)
                    {
                        executor.SendMessage("Broadcaster has been signed to duty!");
                    }
                    else
                    {
                        executor.SendMessage("Broadcaster has been fired, what a shame!");
                    }
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
