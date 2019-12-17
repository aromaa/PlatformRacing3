using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match
{
    public readonly struct UpdatePacketIncomingPacket
    {
        public readonly UpdateStatus Status;

        public readonly double X;
        public readonly double Y;

        public readonly float VelX;
        public readonly float VelY;

        public readonly byte ScaleX;

        public readonly bool Space;
        public readonly bool Left;
        public readonly bool Right;
        public readonly bool Up;
        public readonly bool Down;

        public readonly bool Hurt;

        public readonly int Speed;
        public readonly int Accel;
        public readonly int Jump;

        public readonly int Rotation;
        public readonly uint Life;
        public readonly uint Coins;

        public readonly string Team;
        public readonly string Item;

        public UpdatePacketIncomingPacket(UpdateStatus status, double x, double y, float velX, float velY, byte scaleX, bool space, bool left, bool right, bool up, bool down, bool hurt, int speed, int accel, int jump, int rotation, uint life, uint coins, string team, string item)
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
