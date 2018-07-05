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
    internal class GivePartCommand : ICommand
    {
        public string Permission => "command.givepart.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length >= 3 && args.Length <= 4)
            {
                PlayerUserData playerUserData = UserManager.TryGetUserDataByNameAsync(args[0]).Result;
                if (playerUserData != null)
                {
                    Part part;
                    if (uint.TryParse(args[2], out uint partId))
                    {
                        part = (Part)partId;
                    }
                    else if (!Enum.TryParse(args[2], ignoreCase: true, out part))
                    {
                        executor.SendMessage($"Unable to find part with name {args[2]}");
                    }

                    bool temp = false;
                    if (args.Length >= 4)
                    {
                        bool.TryParse(args[3], out temp);
                    }

                    switch(args[1])
                    {
                        case "head":
                            {
                                if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                                {
                                    session.UserData.GiveHead(part, temp);
                                }
                                else if (!temp)
                                {
                                    UserManager.GiveHead(playerUserData.Id, part);
                                }
                            }
                            break;
                        case "body":
                            {
                                if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                                {
                                    session.UserData.GiveBody(part, temp);
                                }
                                else if (!temp)
                                {
                                    UserManager.GiveBody(playerUserData.Id, part);
                                }
                            }
                            break;
                        case "feet":
                            {
                                if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                                {
                                    session.UserData.GiveFeet(part, temp);
                                }
                                else if (!temp)
                                {
                                    UserManager.GiveFeet(playerUserData.Id, part);
                                }
                            }
                            break;
                        case "set":
                            {
                                if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
                                {
                                    session.UserData.GiveSet(part, temp);
                                }
                                else if (!temp)
                                {
                                    UserManager.GiveSet(playerUserData.Id, part);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    executor.SendMessage($"Unable to find user named {args[0]}");
                }
            }
            else
            {
                executor.SendMessage("Usage: /givepart [user] [head/body/feet/set] [id/name] [temporaly(false)]");
            }
        }
    }
}
