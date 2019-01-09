using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class BroadcastCommand : ICommand
    {
        public string Permission => "command.broadcast.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            foreach(ClientSession session in PlatformRacing3Server.ClientManager.GetLoggedInUsers())
            {
                session.SendPacket(new AlertOutgoingMessage(string.Join(' ', args.ToArray())));
            }
        }
    }
}
