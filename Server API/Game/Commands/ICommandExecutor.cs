using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server_API.Game.Commands
{
    public interface ICommandExecutor
    {
        uint PermissionRank { get; }

        void SendMessage(string message);

        bool HasPermission(string permission);
    }
}
