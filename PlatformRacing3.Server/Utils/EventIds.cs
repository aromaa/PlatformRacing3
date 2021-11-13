using Microsoft.Extensions.Logging;

namespace PlatformRacing3.Server.Utils;

internal sealed class EventIds
{
	internal static readonly EventId CommandExecutionFailed = new(1, nameof(EventIds.CommandExecutionFailed));
	internal static readonly EventId CommandFeedback = new(2, nameof(EventIds.CommandFeedback));

	internal static readonly EventId MatchServerDrawingFailed = new(3, nameof(EventIds.MatchServerDrawingFailed));

	internal static readonly EventId GuestLoginFailed = new(4, nameof(EventIds.GuestLoginFailed));
	internal static readonly EventId TokenLoginFailed = new(5, nameof(EventIds.TokenLoginFailed));
}