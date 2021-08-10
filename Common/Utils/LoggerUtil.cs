using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Platform_Racing_3_Common.Utils
{
	public static class LoggerUtil
	{
		//TODO: Workaround for the static classes, remove after making them singleton
		public static ILoggerFactory LoggerFactory { get; set; }
	}
}
