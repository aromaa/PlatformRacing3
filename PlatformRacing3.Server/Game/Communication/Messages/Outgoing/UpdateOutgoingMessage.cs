using Net.Buffers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Enums;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class UpdateOutgoingMessage : IMessageOutgoing
{
	private const ushort PACKET_HEADER = 23;

	private MatchPlayer MatchPlayer;

	internal UpdateOutgoingMessage(MatchPlayer matchPlayer)
	{
		this.MatchPlayer = matchPlayer;
	}

	public void Write(ref PacketWriter writer)
	{
		writer.WriteUInt16(UpdateOutgoingMessage.PACKET_HEADER);
		writer.WriteUInt32(this.MatchPlayer.SocketId);
		writer.WriteUInt32((uint)this.MatchPlayer.ToUpdate);
		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.X))
		{
			writer.WriteDouble(this.MatchPlayer.X);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Y))
		{
			writer.WriteDouble(this.MatchPlayer.Y);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.VelX))
		{
			writer.WriteSingle(this.MatchPlayer.VelX);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.VelY))
		{
			writer.WriteSingle(this.MatchPlayer.VelY);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.ScaleX))
		{
			writer.WriteByte(this.MatchPlayer.ScaleX);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Space))
		{
			writer.WriteBool(this.MatchPlayer.Space);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Left))
		{
			writer.WriteBool(this.MatchPlayer.Left);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Right))
		{
			writer.WriteBool(this.MatchPlayer.Right);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Down))
		{
			writer.WriteBool(this.MatchPlayer.Down);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Up))
		{
			writer.WriteBool(this.MatchPlayer.Up);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Speed))
		{
			writer.WriteInt32(this.MatchPlayer.Speed);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Accel))
		{
			writer.WriteInt32(this.MatchPlayer.Accel);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Jump))
		{
			writer.WriteInt32(this.MatchPlayer.Jump);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Rot))
		{
			writer.WriteInt32((int)this.MatchPlayer.Rot);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Item))
		{
			writer.WriteFixedUInt16String(this.MatchPlayer.Item);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Life))
		{
			writer.WriteUInt32(this.MatchPlayer.Life);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Hurt))
		{
			writer.WriteBool(this.MatchPlayer.Hurt);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Coins))
		{
			writer.WriteUInt32(this.MatchPlayer.Coins);
		}

		if (this.MatchPlayer.ToUpdate.HasFlag(UpdateStatus.Team))
		{
			writer.WriteFixedUInt16String(this.MatchPlayer.Team);
		}
	}
}