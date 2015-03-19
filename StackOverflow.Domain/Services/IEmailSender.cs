using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StackOverflow.Domain.Services
{
    public interface IEmailSender
    {
        void SendEmail(string email, string message);
    }

    public class MailgunSender : IEmailSender
    {
        public void SendEmail(string email, string message)
        {
            RestClient client = new RestClient
            {
                BaseUrl = "https://api.mailgun.net/v2",
                Authenticator = new HttpBasicAuthenticator("api",
                    "key-69d50aa72b063d08da35cddadf3c0825")
            };
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox1701f8ce878749b3bd773ae90e1e4232.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox1701f8ce878749b3bd773ae90e1e4232.mailgun.org>");
            request.AddParameter("to", email);
            request.AddParameter("subject", "Password Restauration");
            request.AddParameter("text", message);
            request.Method = Method.POST;
            client.Execute(request);
        }
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
