#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Email.Jobs
{
    public class EmailRequest
    {
        public ICollection<EmailRecipient> Recipients { get; set; }

        public string TemplateId { get; set; }

        public Template TemplateModel { get; set; }
    }
}