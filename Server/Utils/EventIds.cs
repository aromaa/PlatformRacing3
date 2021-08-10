using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Platform_Racing_3_Server.Utils
{
	internal sealed class EventIds
	{
		internal static readonly EventId CommandExecutionFailed = new(1, nameof(EventIds.CommandExecutionFailed));
		internal static readonly EventId CommandFeedback = new(2, nameof(EventIds.CommandFeedback));

		internal static readonly EventId MatchServerDrawingFailed = new(3, nameof(EventIds.MatchServerDrawingFailed));

		internal static readonly EventId GuestLoginFailed = new(4, nameof(EventIds.GuestLoginFailed));
		internal static readonly EventId TokenLoginFailed = new(5, nameof(EventIds.TokenLoginFailed));
	}
}
