using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.Misc
{
    internal class KickCommand : ICommand
    {
        public string Permission => "command.kick.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length >= 1)
            {
                ClientSession target = PlatformRacing3Server.ClientManager.GetClientSessionByUsername(args[0]);
                if (target != null)
                {
                    if (target.PermissionRank > executor.PermissionRank)
                    {
                        executor.SendMessage("You do not have permissions to kick this user");

                        return;
                    }

                    if (args.Length == 1)
                    {
                        target.Disconnect("You got kicked for absolute no reason, I bet some staff must hate you");
                    }
                    else
                    {
                        target.Disconnect(string.Join(' ', args[1..].ToArray()));
                    }
                }
                else
                {
                    executor.SendMessage($"Unable to find user online named {args[0]}");
                }
            }
            else
            {
                executor.SendMessage("Usage: /kick [user] [reason(empty)]");
            }
        }
    }
}
