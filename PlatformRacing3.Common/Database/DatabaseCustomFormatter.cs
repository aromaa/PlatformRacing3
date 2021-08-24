using Npgsql;
using Platform_Racing_3_Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Database
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
