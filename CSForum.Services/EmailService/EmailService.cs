using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Shared.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string emailAddress;
        private readonly string password;
        public EmailService(string password, string email)
        {
            this.password = password;
            emailAddress = email;
        }

        public async Task SendMessage<T>(T mail) where T : Email
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(mail.senderName, emailAddress));
            message.To.Add(MailboxAddress.Parse(mail.receiverEmail));

            message.Subject = mail.subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mail.htmlContent;
            bodyBuilder.TextBody = mail.bodyContent;

            message.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, true);
                await smtpClient.AuthenticateAsync(emailAddress, password);
                await smtpClient.SendAsync(message);
            }
        }
    }
}
