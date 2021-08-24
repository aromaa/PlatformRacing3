using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal sealed class AlertCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public AlertCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public string Permission => "command.alert.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length >= 2)
            {
                ClientSession target = this.clientManager.GetClientSessionByUsername(args[0]);
                if (target != null)
                {
                    target.SendPacket(new AlertOutgoingMessage(string.Join(' ', args[1..].ToArray())));
                }
                else
                {
                    executor.SendMessage($"Unable to find user online named {args[0]}");
                }
            }
            else
            {
                executor.SendMessage("Usage: /alert [user] [message]");
            }
        }
    }
}
