using log4net;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Executors
{
    internal class ConsoleCommandExecutor : ICommandExecutor
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public uint PermissionRank => uint.MaxValue;

        public void SendMessage(string message)
        {
            ConsoleCommandExecutor.Logger.Info(message);
        }

        public bool HasPermission(string permission) => true;
    }
}
