namespace PlatformRacing3.Server.Game.Match;

internal class MultiplayerMatchSession
{
	internal MultiplayerMatch Match { get; }
	internal MatchPlayer MatchPlayer { get; private set; }

	internal MultiplayerMatchSession(MultiplayerMatch match, MatchPlayer player)
	{
		this.Match = match;
		this.MatchPlayer = player;
	}

	internal void Forfiet()
	{
		this.MatchPlayer = null;
	}
}