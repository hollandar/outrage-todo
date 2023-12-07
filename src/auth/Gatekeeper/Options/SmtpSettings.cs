namespace Gatekeeper.Options
{
    public class SmtpSettings
    {
        public string? Host { get; set; } = "localhost";
        public int Port { get; set; } = 25;
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public bool EnableSsl { get; set; } = false;
        public string? SenderName { get; set; } = "Identity";
        public string? SenderAddress { get; set; } = "identity@site.com";

    }
}
