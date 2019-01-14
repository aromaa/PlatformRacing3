using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class TournamentCommand : ICommand
    {
        public string Permission => "command.tournament.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                session.HostTournament = true;

                executor.SendMessage("The next match you host will be hosted as tournament");
            }
            else
            {
                executor.SendMessage("This command may only be executed by client session");
            }
        }
    }
}
