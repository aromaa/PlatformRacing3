using PlatformRacing3.Common.Customization;

namespace PlatformRacing3.Common.Extensions;

public static class HatExtensions
{
	public static bool IsStaffOnly(this Hat hat)
	{
		return hat switch
		{
			Hat.Cowboy or Hat.Crown or Hat.Undefined or hat.Extraterrestrial or hat.Extraterrestrial2 => true,

			_ => false,
		};
	}
}
