using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Platform_Racing_3_Server.Utils;

namespace Platform_Racing_3_Server.Game.Commands.Executors
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
