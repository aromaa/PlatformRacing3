using System;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Misc
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
