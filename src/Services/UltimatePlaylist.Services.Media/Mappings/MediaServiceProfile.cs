#region Usings

using AutoMapper;
using UltimatePlaylist.Services.Common.Models.Media.Events;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Media.Mappings
{
    public class MediaServiceProfile : Profile
    {
        public MediaServiceProfile()
        {
            CreateMap(typeof(EventGridEvent<>), typeof(MediaServiceEventGridEvent<>))
                .ForMember(nameof(MediaServiceEventGridEvent<object>.JobName), opts => opts.Ignore());
        }
    }
}
