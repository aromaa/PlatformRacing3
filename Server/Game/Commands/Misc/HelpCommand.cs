using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class HelpCommand : ICommand
    {
        public string Permission => null;

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                executor.SendMessage("<b><u>Developer / Administrator / Manager</u></b>"

                    + "\n\n- /addhat [hat id]"
                    + "- /broadcaster"
                    + "- /broadcast"
                    + "- /fakeprize [type] [prize id]"
                    + "- /spawnaliens [alien amount]"
                    + "- /teleport [x] [y]"
                    + "- /alert [username] [message]"
                    + "- /kick [username] [reason]"
                    + "- /shutdown"
                    + "- /tournament"
                    + "- /givebonusexp [username] [amount]"
                    + "- /givehat [username] [hat id]"
                    + "- /givepart [username] [part id]"

                    + "\n\n<b><u>Everyone</u></b>"

                    + "- /help"
                    + "- /hello"
                    + "- /setworkers [worker amount]");
            }
            else
            {
                executor.SendMessage("This command may only be executed by client session!");
            }
        }
    }
}
