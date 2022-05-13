using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Policy;

namespace testmvc.Models
{
    public class EmailServices : IEmailServices
    {

        private const string EmailTempPath = @"emailtempletes/{0}.html";
        private readonly SMTPconfig _smtpConfig;
        public EmailServices(IOptions<SMTPconfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendResetPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = "Reset Password";
        



            await SendEmail(userEmailOptions);
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {

            MailMessage mailMessage = new MailMessage {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHtml

            };

            foreach (var item in userEmailOptions.ToEmails)
            {
                mailMessage.To.Add(item);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient {
                Host = _smtpConfig.host,
                Port = _smtpConfig.port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential,

            };

     

            mailMessage.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mailMessage);

        }

   
    }
}
