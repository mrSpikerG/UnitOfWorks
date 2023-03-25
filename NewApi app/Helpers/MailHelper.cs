using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace NewApi_app.Helpers {
    public class MailHelper {

        public async Task SendEmailAsync(string email, string subject, string message) {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", ConfigurationManager.AppSetting["Mail"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = message
            };

            using (var client = new SmtpClient()) {
                await client.ConnectAsync(ConfigurationManager.AppSetting["MailConnect:SMPTName"], int.Parse(ConfigurationManager.AppSetting["MailConnect:SMPTPort"]), SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(ConfigurationManager.AppSetting["Mail"], ConfigurationManager.AppSetting["MailPassword"]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

        }
    }
}
