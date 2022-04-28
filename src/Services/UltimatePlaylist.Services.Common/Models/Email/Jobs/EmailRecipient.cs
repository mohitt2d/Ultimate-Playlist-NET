namespace UltimatePlaylist.Services.Common.Models.Email.Jobs
{
    public class EmailRecipient
    {
        public EmailRecipient(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}