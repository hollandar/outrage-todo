using Gatekeeper.Data;
using Gatekeeper.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Gatekeeper.Services
{
    public class IdentityEmailSenderService : IEmailSender<ApplicationUser>
    {
        private readonly IOptions<SmtpSettings> options;

        public IdentityEmailSenderService(IOptions<SmtpSettings> options)
        {
            this.options = options;
        }

        protected async Task Send(string email, string subject, string body, bool isHtml = false)
        {
            var smtpClient = new SmtpClient(options.Value.Host, options.Value.Port);
            if (!String.IsNullOrWhiteSpace(options.Value.Username))
            {
                smtpClient.Credentials = new NetworkCredential(options.Value.Username, options.Value.Password);
            }
            else
            {
                smtpClient.UseDefaultCredentials = true;
            }
            
            smtpClient.EnableSsl = options.Value.EnableSsl;

            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress(options.Value.SenderAddress, options.Value.SenderName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            await smtpClient.SendMailAsync(message);

        }
        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            await Send(email, "Confirmation Link", confirmationLink);
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            await Send(email, "Reset Code", resetCode);
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            await Send(email, "Reset Link", resetLink);
        }
    }
}
