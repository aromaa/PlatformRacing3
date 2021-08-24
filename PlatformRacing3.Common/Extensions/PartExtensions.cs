using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Extensions
{
    public static class PartExtensions
    {
        public static bool IsStaffOnly(this Part part)
        {
			return part switch
			{
				Part.Invisible or Part.MEME or Part.Steve or Part.Rat => true,

				_ => false,
			};
		}

        public static bool IsTournamentPrize(this Part part)
        {
			return part switch
			{
				Part.Hoodie or Part.Cheetah or Part.Cyborg => true,

				_ => false,
			};
		}
    }
}
