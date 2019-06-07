using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class UpdateOutgoingMessage : IMessageOutgoing
    {
        private const ushort PACKET_HEADER = 23;

        private byte[] Bytes { get; }

        internal UpdateOutgoingMessage(MatchPlayer matchPlayer)
        {
            UpdateStatus status = matchPlayer.ToUpdate;

            ServerMessage message = new ServerMessage();
            message.WriteUShort(UpdateOutgoingMessage.PACKET_HEADER);
            message.WriteUInt(matchPlayer.SocketId);
            message.WriteUInt((uint)status);
            if (status.HasFlag(UpdateStatus.X))
            {
                message.WriteDouble(matchPlayer.X);
            }

            if (status.HasFlag(UpdateStatus.Y))
            {
                message.WriteDouble(matchPlayer.Y);
            }

            if (status.HasFlag(UpdateStatus.VelX))
            {
                message.WriteFloat(matchPlayer.VelX);
            }

            if (status.HasFlag(UpdateStatus.VelY))
            {
                message.WriteFloat(matchPlayer.VelY);
            }

            if (status.HasFlag(UpdateStatus.ScaleX))
            {
                message.WriteByte(matchPlayer.ScaleX);
            }

            if (status.HasFlag(UpdateStatus.Space))
            {
                message.WriteBool(matchPlayer.Space);
            }

            if (status.HasFlag(UpdateStatus.Left))
            {
                message.WriteBool(matchPlayer.Left);
            }

            if (status.HasFlag(UpdateStatus.Right))
            {
                message.WriteBool(matchPlayer.Right);
            }

            if (status.HasFlag(UpdateStatus.Down))
            {
                message.WriteBool(matchPlayer.Down);
            }

            if (status.HasFlag(UpdateStatus.Up))
            {
                message.WriteBool(matchPlayer.Up);
            }

            if (status.HasFlag(UpdateStatus.Speed))
            {
                message.WriteInt(matchPlayer.Speed);
            }

            if (status.HasFlag(UpdateStatus.Accel))
            {
                message.WriteInt(matchPlayer.Accel);
            }

            if (status.HasFlag(UpdateStatus.Jump))
            {
                message.WriteInt(matchPlayer.Jump);
            }

            if (status.HasFlag(UpdateStatus.Rot))
            {
                message.WriteInt((int)matchPlayer.Rot);
            }

            if (status.HasFlag(UpdateStatus.Item))
            {
                message.WriteString(matchPlayer.Item);
            }

            if (status.HasFlag(UpdateStatus.Life))
            {
                message.WriteUInt(matchPlayer.Life);
            }

            if (status.HasFlag(UpdateStatus.Hurt))
            {
                message.WriteBool(matchPlayer.Hurt);
            }

            if (status.HasFlag(UpdateStatus.Coins))
            {
                message.WriteUInt(matchPlayer.Coins);
            }

            if (status.HasFlag(UpdateStatus.Team))
            {
                message.WriteString(matchPlayer.Team);
            }

            this.Bytes = message.GetBytes();
        }

        public byte[] GetBytes() => this.Bytes;
    }
}
