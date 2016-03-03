using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace JosephGuadagno.Utilities.Web
{
    public static class Email
    {
        /// <summary>
        ///     Sends the email.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void SendEmail(string to, string subject, string body)
        {
            SendEmail(to, subject, body, ConfigurationManager.AppSettings["emailFrom"]);
        }

        /// <summary>
        ///     Sends the email.
        /// </summary>
        /// <param name="to">A list of emails address to send to.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void SendEmail(List<string> to, string subject, string body)
        {
            to.ForEach(e => SendEmail(e, subject, body, ConfigurationManager.AppSettings["emailFrom"]));
        }

        /// <summary>
        ///     Sends the email.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="from">The person the email is from.</param>
        public static void SendEmail(string to, string subject, string body, string from)
        {
            MailMessage mailMessage = new MailMessage {Sender = new MailAddress(from)};
            mailMessage.From = mailMessage.Sender;
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
            smtpClient.Send(mailMessage);
        }
    }
}