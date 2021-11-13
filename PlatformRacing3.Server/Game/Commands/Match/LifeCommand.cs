using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match
{
	internal sealed class LifeCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public LifeCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public string Permission => "command.life.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                executor.SendMessage("Usage: /life [amount] <target>");

                return;
            }

            if (!uint.TryParse(args[0], out uint amount))
            {
                executor.SendMessage("The amount must be unsigned integer");

                return;
            }

            MultiplayerMatchSession matchSession;
            if (args.Length >= 2)
            {
                ClientSession target = this.clientManager.GetClientSessionByUsername(args[1]);
                if (target == null)
                {
                    executor.SendMessage("The target was not found");

                    return;
                }

                matchSession = target.MultiplayerMatchSession;
            }
            else if (executor is ClientSession client)
            {
                matchSession = client.MultiplayerMatchSession;
            }
            else
            {
                executor.SendMessage("No valid target was found");

                return;
            }

            if (matchSession != null && matchSession.Match != null && matchSession.MatchPlayer != null)
            {
                matchSession.MatchPlayer.Life = amount;

                if (matchSession.MatchPlayer.GetUpdatePacket(out UpdateOutgoingPacket packet))
                {
                    matchSession.Match.SendPacket(packet);
                }
            }
            else
            {
                executor.SendMessage("The target is not currently in a match");
            }
        }
    }
}
