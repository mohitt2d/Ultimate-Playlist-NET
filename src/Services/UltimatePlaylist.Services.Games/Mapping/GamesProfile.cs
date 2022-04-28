#region Usings

using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Games.Models.Lottery;
using UltimatePlaylist.Services.Common.Models.Games;
using UltimatePlaylist.Services.Common.Models.UserManagment;

#endregion

namespace UltimatePlaylist.Services.Games.Mapping
{
    public class GamesProfile : Profile
    {
        public GamesProfile()
        {
            CreateMap<WinningEntity, WinnerProfileReadServiceModel>()
                .ForMember(p => p.ExternalId, opt => opt.MapFrom(m => m.Winner.ExternalId))
                .ForMember(p => p.WinnerFullName, opt => opt.MapFrom(m => $"{m.Winner.Name} {m.Winner.LastName}"))
                .ForMember(p => p.WinnerAvatarUrl, opt => opt.MapFrom(m => m.Winner.AvatarFile.Url))
                .ForMember(p => p.WinnerUsername, opt => opt.MapFrom(m => m.Winner.UserName));

            CreateMap<WinningEntity, UserWinningReadServicModel>();

            CreateMap<UltimatePayoutWinnerReadServiceModel, WinnerProfileReadServiceModel>();

            CreateMap<LotteryWinningNumbersReadServiceModel, UltimatePayoutEntity>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.ExternalId, opt => opt.Ignore())
                .ForMember(p => p.GameDate, opt => opt.Ignore())
                .ForMember(p => p.IsFinished, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.Created, opt => opt.Ignore())
                .ForMember(p => p.Updated, opt => opt.Ignore())
                .ForMember(p => p.Reward, opt => opt.Ignore());

            CreateMap<WinningEntity, PlayerPaymentManagementListItemReadServiceModel>()
                .ForMember(p => p.WinningExternalId, opt => opt.MapFrom(m => m.ExternalId))
                .ForMember(p => p.WinningDate, opt => opt.MapFrom(m => m.Created))
                .ForMember(p => p.UserName, opt => opt.MapFrom(m => m.Winner.UserName))
                .ForMember(p => p.ExternalId, opt => opt.MapFrom(m => m.Winner.ExternalId))
                .ForMember(p => p.BirthDate, opt => opt.MapFrom(m => m.Winner.BirthDate))
                .ForMember(p => p.ImageUrl, opt => opt.MapFrom(m => m.Winner.AvatarFile.Url))
                .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(m => m.Winner.PhoneNumber))
                .ForMember(p => p.Email, opt => opt.MapFrom(m => m.Winner.Email))
                .ForMember(p => p.AgeVerification, opt => opt.MapFrom(m => m.Winner.IsAgeVerified))
                .ForMember(p => p.Name, opt => opt.MapFrom(m => m.Winner.Name))
                .ForMember(p => p.LastName, opt => opt.MapFrom(m => m.Winner.LastName))
                .ForMember(p => p.PaymentStatus, opt => opt.MapFrom(m => m.Status))
                .ForMember(p => p.WinType, opt => opt.MapFrom(m => m.Game.Type));

            CreateMap<WinningEntity, PlayerPaymentReadServiceModel>()
                .ForMember(p => p.PlayerId, opt => opt.MapFrom(m => m.Winner.ExternalId))
                .ForMember(p => p.FirstName, opt => opt.MapFrom(m => m.Winner.Name))
                .ForMember(p => p.LastName, opt => opt.MapFrom(m => m.Winner.LastName))
                .ForMember(p => p.Phone, opt => opt.MapFrom(m => m.Winner.PhoneNumber))
                .ForMember(p => p.Email, opt => opt.MapFrom(m => m.Winner.Email))
                .ForMember(p => p.Zip, opt => opt.MapFrom(m => m.Winner.ZipCode))
                .ForMember(p => p.BirthDate, opt => opt.MapFrom(m => m.Winner.BirthDate))
                .ForMember(p => p.DateOfWinning, opt => opt.MapFrom(m => m.Created))
                .ForMember(p => p.Prize, opt => opt.MapFrom(m => m.Amount));
        }
    }
}
