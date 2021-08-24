using Net.Communication.Attributes;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Enums;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers.Match
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    internal class UpdateIncomingHandler : AbstractIncomingClientSessionPacketHandler<UpdatePacketIncomingPacket>
    {
        internal override void Handle(ClientSession session, in UpdatePacketIncomingPacket packet)
        {
            MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
            if (matchPlayer != null)
            {
                if (packet.Status.HasFlag(UpdateStatus.X))
                {
                    matchPlayer.X = packet.X;
                }

                if (packet.Status.HasFlag(UpdateStatus.Y))
                {
                    matchPlayer.Y = packet.Y;
                }

                if (packet.Status.HasFlag(UpdateStatus.VelX))
                {
                    matchPlayer.VelX = packet.VelX;
                }

                if (packet.Status.HasFlag(UpdateStatus.VelY))
                {
                    matchPlayer.VelY = packet.VelY;
                }

                if (packet.Status.HasFlag(UpdateStatus.ScaleX))
                {
                    matchPlayer.ScaleX = packet.ScaleX;
                }

                if (packet.Status.HasFlag(UpdateStatus.Space))
                {
                    matchPlayer.Space = packet.Space;
                }

                if (packet.Status.HasFlag(UpdateStatus.Left))
                {
                    matchPlayer.Left = packet.Left;
                }

                if (packet.Status.HasFlag(UpdateStatus.Right))
                {
                    matchPlayer.Right = packet.Right;
                }

                if (packet.Status.HasFlag(UpdateStatus.Down))
                {
                    matchPlayer.Down = packet.Down;
                }

                if (packet.Status.HasFlag(UpdateStatus.Up))
                {
                    matchPlayer.Up = packet.Up;
                }

                if (packet.Status.HasFlag(UpdateStatus.Speed))
                {
                    matchPlayer.Speed = packet.Speed;
                }

                if (packet.Status.HasFlag(UpdateStatus.Accel))
                {
                    matchPlayer.Accel = packet.Accel;
                }

                if (packet.Status.HasFlag(UpdateStatus.Jump))
                {
                    matchPlayer.Jump = packet.Jump;
                }

                if (packet.Status.HasFlag(UpdateStatus.Rot))
                {
                    matchPlayer.Rot = packet.Rotation;
                }

                if (packet.Status.HasFlag(UpdateStatus.Item))
                {
                    matchPlayer.Item = packet.Item;
                }

                if (packet.Status.HasFlag(UpdateStatus.Life))
                {
                    matchPlayer.Life = packet.Life;
                }

                if (packet.Status.HasFlag(UpdateStatus.Hurt))
                {
                    matchPlayer.Hurt = packet.Hurt;
                }

                if (packet.Status.HasFlag(UpdateStatus.Coins))
                {
                    matchPlayer.Coins = packet.Coins;
                }

                if (packet.Status.HasFlag(UpdateStatus.Team))
                {
                    matchPlayer.Team = packet.Team;
                }

                matchPlayer.Match.SendUpdateIfRequired(session, matchPlayer);
            }
        }
    }
}
