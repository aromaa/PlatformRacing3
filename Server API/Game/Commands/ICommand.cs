using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server_API.Game.Commands
{
    public interface ICommand
    {
        void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args);
    }
}
