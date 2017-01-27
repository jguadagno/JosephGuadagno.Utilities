using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace JosephGuadagno.Utilities.Web
{
    public class EmailSender
    {
        public EmailSender()
        {
            EmailConfiguration = EmailConfiguration.GetSettingFromConfigurationFile();
        }

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            EmailConfiguration = emailConfiguration;
        }

        public EmailConfiguration EmailConfiguration { get; set; }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="fromUser">The person the email is from.</param>
        public void SendEmail(MailAddress fromUser, string to, string subject, string body)
        {
            SendEmail(fromUser, new MailAddress(to), subject, body);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="toUserList">A list of emails address to send to.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="fromUser">The person the email is from.</param>
        public void SendEmail(MailAddress fromUser, List<string> toUserList, string subject, string body)
        {
            toUserList.ForEach(user => SendEmail(fromUser, new MailAddress(user), subject, body));
        }

        /// <summary>
        /// Sends the email
        /// </summary>
        /// <param name="fromUser">The person the email is from.</param>
        /// <param name="toUser">The person the email is to.</param>
        /// <param name="subject">The subject for the email.</param>
        /// <param name="body">The body of the email.</param>
        public void SendEmail(MailAddress fromUser, MailAddress toUser, string subject, string body)
        {
            var sender = new MailAddress(EmailConfiguration.UserName, EmailConfiguration.DisplayName);
            MailMessage mailMessage = new MailMessage();

            mailMessage.ReplyToList.Add(fromUser);
            mailMessage.Sender = mailMessage.From = sender;
            mailMessage.To.Add(toUser);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient();
            if (!string.IsNullOrEmpty(EmailConfiguration.Host))
            {
                smtpClient.Host = EmailConfiguration.Host;
            }
            if (EmailConfiguration.Port.HasValue && EmailConfiguration.Port != 0)
            {
                smtpClient.Port = EmailConfiguration.Port.Value;
            }
            if (!string.IsNullOrEmpty(EmailConfiguration.UserName))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(EmailConfiguration.UserName, EmailConfiguration.Password);
            }
            smtpClient.Send(mailMessage);
        }
    }
}