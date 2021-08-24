using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Net.Buffers;
using Net.Communication.Attributes;
using Net.Communication.Incoming.Parser;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Parsers.Match
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(67u)]
    internal class UpdatePacketParser : IIncomingPacketParser<UpdatePacketIncomingPacket>
    {
        public UpdatePacketIncomingPacket Parse(ref PacketReader reader)
        {
            UpdateStatus status = (UpdateStatus)reader.ReadUInt32();

            return new UpdatePacketIncomingPacket(
                status: status,

                x: status.HasFlag(UpdateStatus.X) ? reader.ReadDouble() : 0,
                y: status.HasFlag(UpdateStatus.Y) ? reader.ReadDouble() : 0,

                velX: status.HasFlag(UpdateStatus.VelX) ? reader.ReadSingle() : 0,
                velY: status.HasFlag(UpdateStatus.VelY) ? reader.ReadSingle() : 0,

                scaleX: status.HasFlag(UpdateStatus.ScaleX) ? reader.ReadByte() : (byte)0,

                space: status.HasFlag(UpdateStatus.Space) ? reader.ReadBool() : false,

                left: status.HasFlag(UpdateStatus.Left) ? reader.ReadBool() : false,
                right: status.HasFlag(UpdateStatus.Right) ? reader.ReadBool() : false,

                down: status.HasFlag(UpdateStatus.Down) ? reader.ReadBool() : false,
                up: status.HasFlag(UpdateStatus.Up) ? reader.ReadBool() : false,

                speed: status.HasFlag(UpdateStatus.Speed) ? reader.ReadInt32() : 0,
                accel: status.HasFlag(UpdateStatus.Accel) ? reader.ReadInt32() : 0,
                jump: status.HasFlag(UpdateStatus.Jump) ? reader.ReadInt32() : 0,

                rotation: status.HasFlag(UpdateStatus.Rot) ? reader.ReadInt32() : 0,

                item: status.HasFlag(UpdateStatus.Item) ? reader.ReadFixedUInt16String() : null,

                life: status.HasFlag(UpdateStatus.Life) ? reader.ReadUInt32() : 0,
                hurt: status.HasFlag(UpdateStatus.Hurt) ? reader.ReadBool() : false,

                coins: status.HasFlag(UpdateStatus.Coins) ? reader.ReadUInt32() : 0,
                team: status.HasFlag(UpdateStatus.Team) ? reader.ReadFixedUInt16String() : null
            );
        }
    }
}
