namespace PlatformRacing3.Server.Game.Match
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
