using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.User
{
    internal class GiveBonusExpCommand : ICommand
    {
        public string Permission => "command.givebonusexp.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length == 2)
            {
                PlayerUserData playerUserData = UserManager.TryGetUserDataByNameAsync(args[0]).Result;
                if (playerUserData != null)
                {
                    if (!uint.TryParse(args[1], out uint amount))
                    {
                        executor.SendMessage("The amount must be unsigned integer");

                        return;
                    }
                    
                    if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                    {
                        session.UserData.GiveBonusExp(amount);
                    }
                    else
                    {
                        playerUserData.GiveBonusExp(amount);
                    }
                }
                else
                {
                    executor.SendMessage($"Unable to find user named {args[0]}");
                }
            }
            else
            {
                executor.SendMessage("Usage: /givebonusexp [user] [amount]");
            }
        }
    }
}
