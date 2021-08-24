namespace PlatformRacing3.Common.Extensions
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
