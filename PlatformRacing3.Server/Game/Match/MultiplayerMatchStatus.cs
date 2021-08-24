using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    public enum MultiplayerMatchStatus
    {
        PreparingForStart,
        ServerDrawing,
        WaitingForUsersToJoin,
        WaitingForUsersToDraw,
        Starting,
        Ongoing,
        Ended,
        Died,
    }
}
