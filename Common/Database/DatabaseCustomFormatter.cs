using Npgsql;
using Platform_Racing_3_Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Database
{
    internal class DatabaseCustomFormatter : ICustomFormatter
    {
        internal static readonly object DeleteParamReference = new object();

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            NpgsqlParameter param = (NpgsqlParameter)arg;
            if (format == "unsafe" || (!param.Value.GetType().IsEnum && param.Value.IsNumericType()))
            {
                try
                {
                    return param.Value.ToString();
                }
                finally
                {
                    param.Value = DatabaseCustomFormatter.DeleteParamReference;
                }
            }

            return "@" + param.ParameterName;
        }
    }
}
