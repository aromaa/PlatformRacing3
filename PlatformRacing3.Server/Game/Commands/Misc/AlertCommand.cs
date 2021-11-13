using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Commands.Misc
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
