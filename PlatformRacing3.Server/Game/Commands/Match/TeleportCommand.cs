using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match
{
	internal sealed class TeleportCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public TeleportCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public string Permission => "command.teleport.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                executor.SendMessage("Usage: /teleport [x] [y] <target>");

                return;
            }

            if (!double.TryParse(args[0], out double x))
            {
                executor.SendMessage("The x must be double");

                return;
            }

            if (!double.TryParse(args[1], out double y))
            {
                executor.SendMessage("The y must be double");

                return;
            }

            MultiplayerMatchSession matchSession;
            if (args.Length >= 3)
            {
                ClientSession target = this.clientManager.GetClientSessionByUsername(args[2]);
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
                //40 is for block size in pixels
                matchSession.MatchPlayer.X += x * 40;
                matchSession.MatchPlayer.Y -= y * 40;

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
