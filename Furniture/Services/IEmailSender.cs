using MimeKit;
using MailKit.Net.Smtp;

namespace Furniture.Services
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string text, bool contact);
    }
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Send(string to, string subject, string text,bool contact)
        {
            using (var smtp = new SmtpClient())
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["Email:Email"]));
                email.To.Add(MailboxAddress.Parse(to));
                if(contact)
                {
                    email.Subject=subject;
                }
                else
                {
					email.Subject = subject;
				}
               
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = text };

                smtp.Connect(_configuration["Email:Server"], Convert.ToInt32(_configuration["Email:Port"]), true);
                smtp.Authenticate(_configuration["Email:Email"], _configuration["Email:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
