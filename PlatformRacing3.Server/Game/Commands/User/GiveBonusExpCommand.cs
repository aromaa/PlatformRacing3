using PlatformRacing3.Common.User;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.User
{
	internal sealed class GiveBonusExpCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public GiveBonusExpCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

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
                    
                    if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
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
