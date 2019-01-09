using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Match;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Match
{
    internal class AddHatCommand : ICommand
    {
        public string Permission => "command.addhat.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                if (args.Length != 1)
                {
                    executor.SendMessage("Usage: /addhat [hat]");

                    return;
                }

                Hat hat;
                if (uint.TryParse(args[0], out uint hatId))
                {
                    hat = (Hat)hatId;
                }
                else if (!Enum.TryParse(args[0], ignoreCase: true, out hat))
                {
                    executor.SendMessage($"Unable to find part with name {args[0]}");
                }

                MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
                if (matchSession != null && matchSession.Match != null && matchSession.MatchPlayer != null)
                {
                    matchSession.Match.AddHatToPlayer(matchSession.MatchPlayer, hat, session.UserData.CurrentHatColor, spawned: true);
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
