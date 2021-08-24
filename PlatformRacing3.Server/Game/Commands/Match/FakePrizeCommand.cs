using System;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Commands.Match
{
    internal class FakePrizeCommand : ICommand
    {
        public string Permission => "command.fakeprize.use";

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession session)
            {
                if (args.Length != 2)
                {
                    executor.SendMessage("Usage: /fakeprize [category] [id]");

                    return;
                }

                if (!uint.TryParse(args[1], out uint id))
                {
                    executor.SendMessage("The id must be unsigned integer");

                    return;
                }

                MultiplayerMatchSession matchSession = session.MultiplayerMatchSession;
                if (matchSession != null && matchSession.Match != null)
                {;
                    matchSession.Match.SendPacket(new PrizeOutgoingMessage(new MatchPrize(args[0], id), "available"));
                }
                else
                {
                    executor.SendMessage("You need to be in match to use this command");
                }
            }
            else
            {
                executor.SendMessage("This command may only be executed by client session");
            }
        }
    }
}
