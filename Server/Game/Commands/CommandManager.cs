using Platform_Racing_3_Server.Game.Commands.Match;
using Platform_Racing_3_Server.Game.Commands.Misc;
using Platform_Racing_3_Server.Game.Commands.User;
using Platform_Racing_3_Server_API.Game.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Platform_Racing_3_Server.Utils;

namespace Platform_Racing_3_Server.Game.Commands
{
    internal sealed class CommandManager
    {
        private readonly ILogger<CommandManager> logger;

        private Dictionary<string, ICommand> Commands;

        public CommandManager(ILogger<CommandManager> logger, IHostApplicationLifetime applicationLifetime)
        {
            this.logger = logger;

            this.Commands = new Dictionary<string, ICommand>()
            {
                { "hello", new HelloCommand() },
                { "shutdown", new ShutdownCommand(applicationLifetime) },
                { "givepart", new GivePartCommand() },
                { "givehat", new GiveHatCommand() },
                { "broadcast", new BroadcastCommand() },
                { "kick", new KickCommand() },
                { "alert", new AlertCommand() },
                { "addhat", new AddHatCommand() },
                { "fakeprize", new FakePrizeCommand() },
                { "spawnaliens", new SpawnAliensCommand() },
                { "teleport", new TeleportCommand() },
                { "tournament", new TournamentCommand() },
                { "broadcaster", new BroadcasterCommand() },
                { "givebonusexp", new GiveBonusExpCommand() },
                { "life", new LifeCommand() },
                { "item", new ItemCommand() }
            };
        }

        internal bool Execte(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (this.Commands.TryGetValue(label, out ICommand command))
            {
                if (command.Permission == null || executor.HasPermission(command.Permission))
                {
                    try
                    {
                        command.OnCommand(executor, label, args);
                    }
                    catch(Exception ex)
                    {
                        this.logger.LogError(EventIds.CommandExecutionFailed, ex, "Failed to execute command");

                        executor.SendMessage("Critical error while executing the command!");
                    }
                }
                else
                {
                    executor.SendMessage("You are lacking the permissions to use this command!");
                }

                return true;
            }

            return false;
        }
    }
}
