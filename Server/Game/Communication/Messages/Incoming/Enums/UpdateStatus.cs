using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums
{
    [Flags]
    public enum UpdateStatus
    {
        None = 1 << 0,
        X = 1 << 1,
        Y = 1 << 2,
        VelX = 1 << 3,
        VelY = 1 << 4,
        ScaleX = 1 << 5,
        Space = 1 << 6,
        Left = 1 << 7,
        Right = 1 << 8,
        Down = 1 << 9,
        Up = 1 << 10,
        Speed = 1 << 11,
        Accel = 1 << 12,
        Jump = 1 << 13,
        Rot = 1 << 14,
        Item = 1 << 15,
        Life = 1 << 16,
        Hurt = 1 << 17,
        Coins = 1 << 18,
    }
}
