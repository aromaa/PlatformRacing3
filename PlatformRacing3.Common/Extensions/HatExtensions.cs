using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Extensions
{
    public static class HatExtensions
    {
        public static bool IsStaffOnly(this Hat hat)
        {
			return hat switch
			{
				Hat.Cowboy or Hat.Crown => true,

				_ => false,
			};
		}
    }
}
