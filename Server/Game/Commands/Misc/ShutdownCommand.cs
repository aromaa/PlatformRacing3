using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class ShutdownCommand : ICommand
    {
        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor.HasPermission(Permissions.SHUTDOWN))
            {
                Program.Shutdown();
            }
            else
            {
                executor.SendMessage("You lack the permissions to use this command!");
            }
        }
    }
}
