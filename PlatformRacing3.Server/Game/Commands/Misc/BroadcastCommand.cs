using System;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Commands.Misc
{
    internal sealed class BroadcastCommand : ICommand
    {
        private readonly ClientManager clientManager;

        public BroadcastCommand(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public string Permission => "command.broadcast.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            foreach(ClientSession session in this.clientManager.LoggedInUsers)
            {
                session.SendPacket(new AlertOutgoingMessage(string.Join(' ', args.ToArray())));
            }
        }
    }
}
