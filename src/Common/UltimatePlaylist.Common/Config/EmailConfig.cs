#region Usings

using System;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class EmailConfig
    {
        public string FrontendUrl { get; set; }

        public string ConfirmationAction { get; set; }

        public string ConfirmationEmailChangeAction { get; set; }

        public string TemplateRegistrationConfirm { get; set; }

        public string TemplateUpdateProfileConfirm { get; set; }

        public string TemplateUpdateProfileEmailInfo { get; set; }

        public string TemplateResetPassword { get; set; }

        public string ResetPasswordAction { get; set; }

        public string ResetPasswordFrontendAction { get; set; }

        public string ConfirmationFrontendAction { get; set; }

        public string ConfirmationEmailChangeFrontendAction { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        public TimeSpan ConfirmationExpirationTime { get; set; }

        public TimeSpan ResetPasswordExpirationTime { get; set; }

        public string SendGridClientKey { get; set; }

        public string DeeplinkUrl { get; set; }

        public string AppleAppId { get; set; }

        public string AppleStoreId { get; set; }

        public string AndroidAppId { get; set; }
    }
}