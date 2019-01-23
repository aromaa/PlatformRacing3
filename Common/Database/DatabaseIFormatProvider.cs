using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Database
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
