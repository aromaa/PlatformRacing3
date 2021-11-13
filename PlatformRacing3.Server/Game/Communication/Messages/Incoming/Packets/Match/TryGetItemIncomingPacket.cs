namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Match;

internal readonly struct TryGetItemIncomingPacket
{
	internal readonly int X;
	internal readonly int Y;

	internal readonly string Side;
	internal readonly string Item;

	public TryGetItemIncomingPacket(int x, int y, string side, string item)
	{
		this.X = x;
		this.Y = y;
		this.Side = side;
		this.Item = item;
	}
}