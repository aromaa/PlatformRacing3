using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Commands.User
{
    internal class GiveHatCommand : ICommand
    {
        public string Permission => "command.givehat.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length >= 2 && args.Length <= 3)
            {
                PlayerUserData playerUserData = UserManager.TryGetUserDataByNameAsync(args[0]).Result;
                if (playerUserData != null)
                {
                    Hat hat;
                    if (uint.TryParse(args[1], out uint hatId))
                    {
                        hat = (Hat)hatId;
                    }
                    else if (!Enum.TryParse(args[1], ignoreCase: true, out hat))
                    {
                        executor.SendMessage($"Unable to find part with name {args[1]}");
                    }

                    bool temp = false;
                    if (args.Length >= 3)
                    {
                        bool.TryParse(args[2], out temp);
                    }

                    if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                    {
                        session.UserData.GiveHat(hat, temp);
                    }
                    else if (!temp)
                    {
                        UserManager.GiveHat(playerUserData.Id, hat);
                    }
                }
                else
                {
                    executor.SendMessage($"Unable to find user named {args[0]}");
                }
            }
            else
            {
                executor.SendMessage("Usage: /givehat [user] [id/name] [temporaly(false)]");
            }
        }
    }
}
