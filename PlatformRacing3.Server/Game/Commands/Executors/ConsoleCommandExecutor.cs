using Microsoft.Extensions.Logging;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Utils;

namespace PlatformRacing3.Server.Game.Commands.Executors
{
    internal class ConsoleCommandExecutor : ICommandExecutor
    {
        private readonly ILogger<ConsoleCommandExecutor> logger;

        public ConsoleCommandExecutor(ILogger<ConsoleCommandExecutor> logger)
        {
            this.logger = logger;
        }

        public uint PermissionRank => uint.MaxValue;

        public void SendMessage(string message)
        {
            this.logger.LogInformation(EventIds.CommandFeedback, message);
        }

        public bool HasPermission(string permission) => true;
    }
}
