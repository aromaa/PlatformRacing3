using System;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Common.User;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.User
{
    internal sealed class GivePartCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public GivePartCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

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
                                if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
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
                                if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
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
                                if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
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
                                if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
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
