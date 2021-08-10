using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Platform_Racing_3_Web.Utils
{
	internal static class EventIds
	{
		internal static readonly EventId DataAccess2Failed = new(1, nameof(EventIds.DataAccess2Failed));
	}
}
