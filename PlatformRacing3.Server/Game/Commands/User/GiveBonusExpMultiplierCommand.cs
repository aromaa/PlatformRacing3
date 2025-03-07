using PlatformRacing3.Common.User;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;
using System.Globalization;

namespace PlatformRacing3.Server.Game.Commands.User;

internal sealed class GiveBonusExpMultiplierCommand : ICommand
{
	private readonly ClientManager clientManager;

	public GiveBonusExpMultiplierCommand(ClientManager clientManager)
	{
		this.clientManager = clientManager;
	}

	public string Permission => "command.givebonusexpmultiplier.use";

	public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
	{
		if (args.Length == 3)
		{
			PlayerUserData playerUserData = UserManager.TryGetUserDataByNameAsync(args[0]).Result;
			if (playerUserData != null)
			{
				if (!double.TryParse(args[1], CultureInfo.InvariantCulture, out double multiplier))
				{
					executor.SendMessage("The amount must be valid decimal");

					return;
				}

				if (!uint.TryParse(args[2], out uint time))
				{
					executor.SendMessage("The time must be valid unsigned integer");

					return;
				}

				if (this.clientManager.TryGetClientSessionByUserId(playerUserData.Id, out ClientSession session) && session.UserData != null)
				{
					session.UserData.SetBonusExpMultiplier(multiplier, DateTime.UtcNow.AddSeconds(time));
				}
				else
				{
					playerUserData.SetBonusExpMultiplier(multiplier, DateTime.UtcNow.AddSeconds(time));
				}
			}
			else
			{
				executor.SendMessage($"Unable to find user named {args[0]}");
			}
		}
		else
		{
			executor.SendMessage("Usage: /givebonusexpmultiplier [user] [multiplier] [time in seconds]");
		}
	}
}
