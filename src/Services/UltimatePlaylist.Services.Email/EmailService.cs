#region Usings

using System.Net;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Services.Common.Interfaces;
using UltimatePlaylist.Services.Common.Models.Email.Jobs;

#endregion

namespace UltimatePlaylist.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig config;
        private readonly SendGridClient client;

        public EmailService(IOptions<EmailConfig> options)
        {
            config = options.Value;
            client = new SendGridClient(config.SendGridClientKey);
        }

        public async Task SendEmailAsync(EmailRequest email)
        {
            var sendgridMessage = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(
                new EmailAddress(config.SenderEmail, config.SenderName),
                email.Recipients.Select(recipient => new EmailAddress(recipient.Email, recipient.Name)).ToList(),
                email.TemplateId,
                email.TemplateModel);

            var response = await client.SendEmailAsync(sendgridMessage);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var body = await response.Body.ReadAsStringAsync();

                throw new BusinessException($"{ErrorType.ErrorSendingEmail} {body}");
            }
        }
    }
}
