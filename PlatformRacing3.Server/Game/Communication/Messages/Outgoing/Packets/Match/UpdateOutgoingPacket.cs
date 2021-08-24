using Net.Buffers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Enums;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Packets.Match
{
    internal readonly struct UpdateOutgoingPacket : IMessageOutgoing
    {
        private const ushort PACKET_HEADER = 23;

        internal readonly UpdateStatus Status;

        internal readonly MatchPlayer MatchPlayer;

        internal UpdateOutgoingPacket(UpdateStatus status, MatchPlayer matchPlayer)
        {
            this.Status = status;

            this.MatchPlayer = matchPlayer;
        }

        public void Write(ref PacketWriter writer)
        {
            writer.WriteUInt16(UpdateOutgoingPacket.PACKET_HEADER);
            writer.WriteUInt32(this.MatchPlayer.SocketId);
            writer.WriteUInt32((uint)this.Status);

            if (this.Status.HasFlag(UpdateStatus.X))
            {
                writer.WriteDouble(this.MatchPlayer.X);
            }

            if (this.Status.HasFlag(UpdateStatus.Y))
            {
                writer.WriteDouble(this.MatchPlayer.Y);
            }

            if (this.Status.HasFlag(UpdateStatus.VelX))
            {
                writer.WriteSingle(this.MatchPlayer.VelX);
            }

            if (this.Status.HasFlag(UpdateStatus.VelY))
            {
                writer.WriteSingle(this.MatchPlayer.VelY);
            }

            if (this.Status.HasFlag(UpdateStatus.ScaleX))
            {
                writer.WriteByte(this.MatchPlayer.ScaleX);
            }

            if (this.Status.HasFlag(UpdateStatus.Space))
            {
                writer.WriteBool(this.MatchPlayer.Space);
            }

            if (this.Status.HasFlag(UpdateStatus.Left))
            {
                writer.WriteBool(this.MatchPlayer.Left);
            }

            if (this.Status.HasFlag(UpdateStatus.Right))
            {
                writer.WriteBool(this.MatchPlayer.Right);
            }

            if (this.Status.HasFlag(UpdateStatus.Down))
            {
                writer.WriteBool(this.MatchPlayer.Down);
            }

            if (this.Status.HasFlag(UpdateStatus.Up))
            {
                writer.WriteBool(this.MatchPlayer.Up);
            }

            if (this.Status.HasFlag(UpdateStatus.Speed))
            {
                writer.WriteInt32(this.MatchPlayer.Speed);
            }

            if (this.Status.HasFlag(UpdateStatus.Accel))
            {
                writer.WriteInt32(this.MatchPlayer.Accel);
            }

            if (this.Status.HasFlag(UpdateStatus.Jump))
            {
                writer.WriteInt32(this.MatchPlayer.Jump);
            }

            if (this.Status.HasFlag(UpdateStatus.Rot))
            {
                writer.WriteInt32((int)this.MatchPlayer.Rot);
            }

            if (this.Status.HasFlag(UpdateStatus.Item))
            {
                writer.WriteFixedUInt16String(this.MatchPlayer.Item);
            }

            if (this.Status.HasFlag(UpdateStatus.Life))
            {
                writer.WriteUInt32(this.MatchPlayer.Life);
            }

            if (this.Status.HasFlag(UpdateStatus.Hurt))
            {
                writer.WriteBool(this.MatchPlayer.Hurt);
            }

            if (this.Status.HasFlag(UpdateStatus.Coins))
            {
                writer.WriteUInt32(this.MatchPlayer.Coins);
            }

            if (this.Status.HasFlag(UpdateStatus.Team))
            {
                writer.WriteFixedUInt16String(this.MatchPlayer.Team);
            }
        }
    }
}
