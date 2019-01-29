using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class UpdateIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, IncomingMessage message)
        {
            MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
            if (matchPlayer != null)
            {
                UpdateStatus status = (UpdateStatus)message.ReadUInt();
                if (status.HasFlag(UpdateStatus.X))
                {
                    matchPlayer.X = message.ReadDouble();
                }

                if (status.HasFlag(UpdateStatus.Y))
                {
                    matchPlayer.Y = message.ReadDouble();
                }

                if (status.HasFlag(UpdateStatus.VelX))
                {
                    matchPlayer.VelX = message.ReadFloat();
                }

                if (status.HasFlag(UpdateStatus.VelY))
                {
                    matchPlayer.VelY = message.ReadFloat();
                }

                if (status.HasFlag(UpdateStatus.ScaleX))
                {
                    matchPlayer.ScaleX = message.ReadByte();
                }

                if (status.HasFlag(UpdateStatus.Space))
                {
                    matchPlayer.Space = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Left))
                {
                    matchPlayer.Left = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Right))
                {
                    matchPlayer.Right = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Down))
                {
                    matchPlayer.Down = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Up))
                {
                    matchPlayer.Up = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Speed))
                {
                    matchPlayer.Speed = message.ReadInt();
                }

                if (status.HasFlag(UpdateStatus.Accel))
                {
                    matchPlayer.Accel = message.ReadInt();
                }

                if (status.HasFlag(UpdateStatus.Jump))
                {
                    matchPlayer.Jump = message.ReadInt();
                }

                if (status.HasFlag(UpdateStatus.Rot))
                {
                    matchPlayer.Rot = message.ReadInt();
                }

                if (status.HasFlag(UpdateStatus.Item))
                {
                    matchPlayer.Item = message.ReadString();
                }

                if (status.HasFlag(UpdateStatus.Life))
                {
                    matchPlayer.Life = message.ReadUInt();
                }

                if (status.HasFlag(UpdateStatus.Hurt))
                {
                    matchPlayer.Hurt = message.ReadBool();
                }

                if (status.HasFlag(UpdateStatus.Coins))
                {
                    matchPlayer.Coins = message.ReadUInt();
                }

                matchPlayer.Match.SendUpdateIfRequired(session, matchPlayer);
            }
        }
    }
}
