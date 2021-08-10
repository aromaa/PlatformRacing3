using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal sealed class ShutdownCommand : ICommand
    {
        private readonly IHostApplicationLifetime applicationLifetime;

        public ShutdownCommand(IHostApplicationLifetime applicationLifetime)
        {
            this.applicationLifetime = applicationLifetime;
        }

        public string Permission => "command.shutdown.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            this.applicationLifetime.StopApplication();
        }
    }
}
