using FluentResults;
using Microsoft.Extensions.Options;
using NotificationService.Application.Services;
using NotificationService.Contracts.Requests;
using NotificationService.Infrastructure.Common.Settings;
using NotificationService.Infrastructure.Exceptions;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Infrastructure.Common.Services
{
    internal class GmailService : IMailService
    {
        private readonly GmailSenderSettings _gmailSettings;

        public GmailService(IOptions<GmailSenderSettings> gmailSettings) 
        {
            _gmailSettings = gmailSettings.Value;
        }

        public async Task<Result> SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_gmailSettings.Email), 
                Subject =  _gmailSettings.Subject,
                Body = sendEmailRequest.Body
            };

            mailMessage.To.Add(sendEmailRequest.Recipient);

            try
            {
                using var smtpClient = new SmtpClient();
                smtpClient.Host = _gmailSettings.Host;
                smtpClient.Port = _gmailSettings.Port;
                smtpClient.Credentials = new NetworkCredential(_gmailSettings.Email, _gmailSettings.Password);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"Email successfully sent to {sendEmailRequest.Recipient}");
                return Result.Ok();
            }
            catch(Exception ex)
            {
                return Result.Fail(InfrastructureErrors.MailService.UnexpectedError(ex.Message));
            }
        }
    }
}
