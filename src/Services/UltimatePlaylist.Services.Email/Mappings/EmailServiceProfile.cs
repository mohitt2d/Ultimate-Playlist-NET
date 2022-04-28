#region Usings

using AutoMapper;
using UltimatePlaylist.Services.Common.Models.Email;
using UltimatePlaylist.Services.Common.Models.Email.Jobs;

#endregion

namespace UltimatePlaylist.Services.Email.Mappings
{
    public class EmailServiceProfile : Profile
    {
        public EmailServiceProfile()
        {
            CreateMap<ResetPasswordEmailRequset, ResetPasswordEmailTemplate>();
            CreateMap<RegistrationConfirmationRequest, RegistrationConfirmationEmailTemplate>();
            CreateMap<EmailChangeConfirmationRequest, EmailChangeConfirmationEmailTemplate>();
            CreateMap<EmailChangingInfoRequest, EmailChangeConfirmationEmailTemplate>()
                .ForMember(s => s.Token, o => o.Ignore());
        }
    }
}
