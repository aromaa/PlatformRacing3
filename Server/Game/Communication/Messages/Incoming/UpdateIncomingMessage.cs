using System;
using System.Collections.Generic;
using System.Text;
using Net.Communication.Incoming.Helpers;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class UpdateIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, ref PacketReader reader)
        {
            MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
            if (matchPlayer != null)
            {
                UpdateStatus status = (UpdateStatus)reader.ReadUInt32();
                if (status.HasFlag(UpdateStatus.X))
                {
                    matchPlayer.X = reader.ReadDouble();
                }

                if (status.HasFlag(UpdateStatus.Y))
                {
                    matchPlayer.Y = reader.ReadDouble();
                }

                if (status.HasFlag(UpdateStatus.VelX))
                {
                    matchPlayer.VelX = reader.ReadSingle();
                }

                if (status.HasFlag(UpdateStatus.VelY))
                {
                    matchPlayer.VelY = reader.ReadSingle();
                }

                if (status.HasFlag(UpdateStatus.ScaleX))
                {
                    matchPlayer.ScaleX = reader.ReadByte();
                }

                if (status.HasFlag(UpdateStatus.Space))
                {
                    matchPlayer.Space = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Left))
                {
                    matchPlayer.Left = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Right))
                {
                    matchPlayer.Right = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Down))
                {
                    matchPlayer.Down = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Up))
                {
                    matchPlayer.Up = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Speed))
                {
                    matchPlayer.Speed = reader.ReadInt32();
                }

                if (status.HasFlag(UpdateStatus.Accel))
                {
                    matchPlayer.Accel = reader.ReadInt32();
                }

                if (status.HasFlag(UpdateStatus.Jump))
                {
                    matchPlayer.Jump = reader.ReadInt32();
                }

                if (status.HasFlag(UpdateStatus.Rot))
                {
                    matchPlayer.Rot = reader.ReadInt32();
                }

                if (status.HasFlag(UpdateStatus.Item))
                {
                    matchPlayer.Item = reader.ReadFixedString();
                }

                if (status.HasFlag(UpdateStatus.Life))
                {
                    matchPlayer.Life = reader.ReadUInt32();
                }

                if (status.HasFlag(UpdateStatus.Hurt))
                {
                    matchPlayer.Hurt = reader.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Coins))
                {
                    matchPlayer.Coins = reader.ReadUInt32();
                }

                if (status.HasFlag(UpdateStatus.Team))
                {
                    matchPlayer.Team = reader.ReadFixedString();
                }

                matchPlayer.Match.SendUpdateIfRequired(session, matchPlayer);
            }
        }
    }
}
