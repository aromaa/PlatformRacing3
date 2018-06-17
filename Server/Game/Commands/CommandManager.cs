using Platform_Racing_3_Server.Game.Commands.Misc;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands
{
    internal class CommandManager
    {
        private Dictionary<string, ICommand> Commands;

        internal CommandManager()
        {
            this.Commands = new Dictionary<string, ICommand>()
            {
                { "hello", new HelloCommand() },
                { "shutdown", new ShutdownCommand() },
            };
        }

        internal bool Execte(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (this.Commands.TryGetValue(label, out ICommand command))
            {
                try
                {
                    command.OnCommand(executor, label, args);
                }
                catch
                {
                    executor.SendMessage("Critical error while executing the command!");
                }

                return true;
            }

            return false;
        }
    }
}
