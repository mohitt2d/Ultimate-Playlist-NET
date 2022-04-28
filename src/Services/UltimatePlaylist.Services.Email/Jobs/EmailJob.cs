#region Usings

using System.Text.Json;
using System.Web;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Interfaces;
using UltimatePlaylist.Services.Common.Models.Email;
using UltimatePlaylist.Services.Common.Models.Email.Jobs;

#endregion

namespace UltimatePlaylist.Services.Email.Jobs
{
    public class EmailJob
    {
        private readonly EmailConfig EmailConfig;

        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly ILogger<EmailJob> logger;

        public EmailJob(
            ILogger<EmailJob> logger,
            IOptions<EmailConfig> emailConfig,
            IMapper mapper,
            IEmailService emailService)
        {
            EmailConfig = emailConfig.Value;
            this.mapper = mapper;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task SendRegistrationConfirmationAsync(RegistrationConfirmationRequest request)
        {
            var template = mapper.Map<RegistrationConfirmationEmailTemplate>(request);
            template.Url = $"{EmailConfig.DeeplinkUrl}/?link={HttpUtility.UrlEncode(BuildUrl(template, EmailConfig.DeeplinkUrl, EmailConfig.ConfirmationAction))}&apn={EmailConfig.AndroidAppId}&ibi={EmailConfig.AppleAppId}&isi={EmailConfig.AppleStoreId}&ofl={BuildUrl(template, EmailConfig.FrontendUrl, EmailConfig.ConfirmationFrontendAction)}";

            var email = new EmailRequest
            {
                Recipients = new List<EmailRecipient> { new EmailRecipient(request.Email, request.Name) },
                TemplateId = EmailConfig.TemplateRegistrationConfirm,
                TemplateModel = template,
            };

            await emailService.SendEmailAsync(email);
            logger.LogInformation("Send Registration Confirmation Email", email);
        }

        public async Task SendUpdateProfileConfirmationAsync(EmailChangeConfirmationRequest request)
        {
            var template = mapper.Map<EmailChangeConfirmationEmailTemplate>(request);
            template.Url = $"{EmailConfig.DeeplinkUrl}/?link={HttpUtility.UrlEncode(BuildUrl(template, EmailConfig.DeeplinkUrl, EmailConfig.ConfirmationEmailChangeAction))}&apn={EmailConfig.AndroidAppId}&ibi={EmailConfig.AppleAppId}&isi={EmailConfig.AppleStoreId}&ofl={BuildUrl(template, EmailConfig.FrontendUrl, EmailConfig.ConfirmationEmailChangeFrontendAction)}";

            var email = new EmailRequest
            {
                Recipients = new List<EmailRecipient> { new EmailRecipient(request.Email, request.Name) },
                TemplateId = EmailConfig.TemplateUpdateProfileConfirm,
                TemplateModel = template,
            };

            await emailService.SendEmailAsync(email);
            logger.LogInformation("Send Email Changed Confirmation Email", email);
        }

        public async Task SendEmailChangingInfoToOldEmailAddressAsync(EmailChangingInfoRequest request)
        {
            var template = mapper.Map<EmailChangeConfirmationEmailTemplate>(request);

            var email = new EmailRequest
            {
                Recipients = new List<EmailRecipient> { new EmailRecipient(request.Email, request.Name) },
                TemplateId = EmailConfig.TemplateUpdateProfileEmailInfo,
                TemplateModel = template,
            };

            await emailService.SendEmailAsync(email);
            logger.LogInformation("Send Email With Info About Changing Email", email);
        }

        public async Task SendResetPasswordAsync(ResetPasswordEmailRequset request)
        {
            var template = mapper.Map<ResetPasswordEmailTemplate>(request);
            template.Url = BuildUrl(template, EmailConfig.FrontendUrl, EmailConfig.ResetPasswordFrontendAction);

            template.Url = $"{EmailConfig.DeeplinkUrl}/?link={HttpUtility.UrlEncode(BuildUrl(template, EmailConfig.DeeplinkUrl, EmailConfig.ResetPasswordAction))}&apn={EmailConfig.AndroidAppId}&ibi={EmailConfig.AppleAppId}&isi={EmailConfig.AppleStoreId}&ofl={BuildUrl(template, EmailConfig.FrontendUrl, EmailConfig.ResetPasswordFrontendAction)}";

            var email = new EmailRequest
            {
                Recipients = new List<EmailRecipient> { new EmailRecipient(request.Email, request.Name) },
                TemplateId = EmailConfig.TemplateResetPassword,
                TemplateModel = template,
            };

            await emailService.SendEmailAsync(email);
            logger.LogInformation("Send Registration Confirmation Email", email);
        }

        private string BuildUrl(Template template, string url, string action)
        {
            string jsonString = JsonSerializer.Serialize(new
            {
                template.Email,
                template.Token,
            });

            byte[] toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(jsonString);
            var query = $"token={Convert.ToBase64String(toEncodeAsBytes)}";
            var requesturl = $"{url}/{action}?";
            return requesturl + query;
        }
    }
}
