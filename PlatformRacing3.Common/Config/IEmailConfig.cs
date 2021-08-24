using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Config
{
    public interface IEmailConfig
    {
        string SmtpHost { get; set; }
        ushort SmtpPort { get; set; }

        string SmtpUser { get; set; }
        string SmtpPass { get; set; }
    }
}
