namespace PlatformRacing3.Common.Config
{
    public interface IEmailConfig
    {
        string SmtpHost { get; set; }
        ushort SmtpPort { get; set; }

        string SmtpUser { get; set; }
        string SmtpPass { get; set; }
    }
}
