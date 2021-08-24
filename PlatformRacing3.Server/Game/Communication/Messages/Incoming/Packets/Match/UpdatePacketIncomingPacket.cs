using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match
{
    internal readonly struct UpdatePacketIncomingPacket
    {
        public readonly UpdateStatus Status;

        public readonly double X;
        public readonly double Y;

        public readonly float VelX;
        public readonly float VelY;

        public readonly byte ScaleX;

        internal readonly bool Space;
        internal readonly bool Left;
        internal readonly bool Right;
        internal readonly bool Up;
        internal readonly bool Down;

        internal readonly bool Hurt;

        internal readonly int Speed;
        internal readonly int Accel;
        internal readonly int Jump;

        internal readonly int Rotation;
        internal readonly uint Life;
        internal readonly uint Coins;

        internal readonly string Team;
        internal readonly string Item;

        internal UpdatePacketIncomingPacket(UpdateStatus status, double x, double y, float velX, float velY, byte scaleX, bool space, bool left, bool right, bool up, bool down, bool hurt, int speed, int accel, int jump, int rotation, uint life, uint coins, string team, string item)
        {
            this.Status = status;

            this.X = x;
            this.Y = y;

            this.VelX = velX;
            this.VelY = velY;

            this.ScaleX = scaleX;

            this.Space = space;
            this.Left = left;
            this.Right = right;
            this.Up = up;
            this.Down = down;

            this.Hurt = hurt;

            this.Speed = speed;
            this.Accel = accel;
            this.Jump = jump;

            this.Rotation = rotation;
            this.Life = life;
            this.Coins = coins;

            this.Team = team;
            this.Item = item;
        }
    }
}
