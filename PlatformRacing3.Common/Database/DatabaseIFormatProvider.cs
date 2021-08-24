using System;

namespace PlatformRacing3.Common.Database
{
    internal class DatabaseIFormatProvider : IFormatProvider
    {
        internal static DatabaseIFormatProvider Instance { get; } = new DatabaseIFormatProvider();

        public object GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).IsAssignableFrom(formatType))
            {
                return DatabaseCustomFormatter.Instance;
            }

            return null;
        }
    }
}
