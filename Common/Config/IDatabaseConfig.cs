using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Config
{
    public interface IDatabaseConfig
    {
        string DatabaseHost { get; }
        uint DatabasePort { get; }

        string DatabaseUser { get; }
        string DatabasePass { get; }
        string DatabaseName { get; }
    }
}
