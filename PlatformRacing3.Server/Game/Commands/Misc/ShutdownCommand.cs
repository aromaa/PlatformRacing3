using Microsoft.Extensions.Hosting;
using PlatformRacing3.Server.API.Game.Commands;

namespace PlatformRacing3.Server.Game.Commands.Misc
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
