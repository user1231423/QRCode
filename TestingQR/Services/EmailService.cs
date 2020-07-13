using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TestingQR.Models;

namespace TestingQR.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmail(string email, string subject, string message)
        {
            try
            {
                Execute(email, subject, message).Wait();
                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string ToEmail = email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.SenderEmail)
                };

                mail.To.Add(new MailAddress(ToEmail));
                //mail.CC.Add(new MailAddress());

                mail.Subject = "NoReply";
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                    smtpClient.Host = _emailSettings.MailServer;
                    smtpClient.Port = _emailSettings.MailPort; // Google smtp port
                    smtpClient.EnableSsl = _emailSettings.EnableSSL;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;// disable it
                    /// Now specify the credentials 
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);

                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
