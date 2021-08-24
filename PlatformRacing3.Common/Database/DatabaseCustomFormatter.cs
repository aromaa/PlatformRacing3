using System;

namespace PlatformRacing3.Common.Database
{
    internal class DatabaseCustomFormatter : ICustomFormatter
    {
        internal static DatabaseCustomFormatter Instance { get; } = new DatabaseCustomFormatter();

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return arg.ToString();
        }
    }
}
