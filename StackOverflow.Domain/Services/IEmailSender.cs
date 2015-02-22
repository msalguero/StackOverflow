using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Services
{
    public interface IEmailSender
    {
        void SendEmail(string email, string message);
    }

    public class EmailSender : IEmailSender
    {
        public void SendEmail(string email, string message)
        {
            MailMessage mail = new MailMessage("mystackoverflow87@gmail.com", email);
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mystackoverflow87@gmail.com", "stackoverflow87")
            };
            mail.Subject = "Password Recovery";
            mail.Body = message;
            client.Send(mail);
        }
    }
}
