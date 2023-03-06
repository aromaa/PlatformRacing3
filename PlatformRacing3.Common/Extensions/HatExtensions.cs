using PlatformRacing3.Common.Customization;

namespace PlatformRacing3.Common.Extensions;

public static class HatExtensions
{
	public static bool IsStaffOnly(this Hat hat)
	{
		return hat switch
		{
			Hat.Cowboy or Hat.Crown or Hat.Extraterrestrial or Hat.Alien => true,

			_ => false,
		};
	}
}
