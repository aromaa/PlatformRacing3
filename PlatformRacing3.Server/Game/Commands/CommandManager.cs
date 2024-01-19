using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Commands.Match;
using PlatformRacing3.Server.Game.Commands.Misc;
using PlatformRacing3.Server.Game.Commands.Selector;
using PlatformRacing3.Server.Game.Commands.User;
using PlatformRacing3.Server.Utils;

namespace PlatformRacing3.Server.Game.Commands;

internal sealed class CommandManager
{
	private readonly ILogger<CommandManager> logger;

	private readonly ClientManager clientManager;

	private Dictionary<string, ICommand> Commands;
	private Dictionary<string, ICommandTargetSelector> TargetSelectors;

	public CommandManager(ClientManager clientManager, ILogger<CommandManager> logger, IHostApplicationLifetime applicationLifetime)
	{
		this.logger = logger;

		this.clientManager = clientManager;

		this.Commands = new Dictionary<string, ICommand>()
		{
			{ "hello", new HelloCommand() },
			{ "shutdown", new ShutdownCommand(applicationLifetime) },
			{ "givepart", new GivePartCommand(clientManager) },
			{ "givehat", new GiveHatCommand(clientManager) },
			{ "broadcast", new BroadcastCommand(clientManager) },
			{ "kick", new KickCommand(this) },
			{ "alert", new AlertCommand(this) },
			{ "addhat", new AddHatCommand(this) },
			{ "fakeprize", new FakePrizeCommand() },
			{ "spawnaliens", new SpawnAliensCommand() },
			{ "teleport", new TeleportCommand(this) },
			{ "tournament", new TournamentCommand() },
			{ "broadcaster", new BroadcasterCommand() },
			{ "givebonusexp", new GiveBonusExpCommand(clientManager) },
			{ "life", new LifeCommand(this) },
			{ "item", new ItemCommand(this) },
			{ "spectate", new SpectateCommand() },
			{ "mute", new MuteCommand(this) },
			{ "unmute", new UnmuteCommand(this) },
			{ "removehats", new RemoveHatsCommand(this) },
			{ "delaystart", new DelayStartCommand() },
			{ "teleportto", new TeleportToCommand(this) },
			{ "teleporthere", new TeleportHereCommand(this) },
			{ "forfeit", new ForfeitCommand(this) },
			{ "finish", new FinishCommand(this) }
		};

		this.TargetSelectors = new Dictionary<string, ICommandTargetSelector>()
		{
			{ "@online", new OnlineCommandTargetSelector(clientManager) },
			{ "@match", new MatchCommandTargetSelector() },
			{ "@alive", new AliveCommandTargetSelector() },
			{ "@dead", new DeadCommandTargetSelector() },
			{ "@me", new MeCommandTargetSelector() }
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

	internal IEnumerable<ClientSession> GetTargets(ICommandExecutor executor, string selector)
	{
		if (this.TargetSelectors.TryGetValue(selector, out ICommandTargetSelector targetSelector))
		{
			return targetSelector.FindTargets(executor, string.Empty);
		}

		if (this.clientManager.GetClientSessionByUsername(selector) is { } target)
		{
			return new ClientSession[] { target };
		}

		return Array.Empty<ClientSession>();
	}
}