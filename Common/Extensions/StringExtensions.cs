using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Extensions
{
    internal static class StringExtensions
    {
        internal static uint CountCharsSum(this string string_)
        {
            uint sum = 0;
            foreach(char char_ in string_)
            {
                sum += char_;
            }

            return sum;
        }
    }
}
