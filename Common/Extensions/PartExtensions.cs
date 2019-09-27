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
            switch(part)
            {
                case Part.Invisible:
                case Part.MEME:
                case Part.Steve:
                case Part.Rat:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsTournamentPrize(this Part part)
        {
            switch(part)
            {
                case Part.Hoodie:
                case Part.Cheetah:
                case Part.Cyborg:
                    return true;
                default:
                    return false;
            }
        }
    }
}
