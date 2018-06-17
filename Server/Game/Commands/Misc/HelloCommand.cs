using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class HelloCommand : ICommand
    {
        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession client)
            {
                executor.SendMessage($"Hello, {client.UserData.Username} !");
            }
            else
            {
                executor.SendMessage("Hello, someone!");
            }
        }
    }
}
