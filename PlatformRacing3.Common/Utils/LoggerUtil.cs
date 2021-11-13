using Microsoft.Extensions.Logging;

namespace PlatformRacing3.Common.Utils;

public static class LoggerUtil
{
	//TODO: Workaround for the static classes, remove after making them singleton
	public static ILoggerFactory LoggerFactory { get; set; }
}