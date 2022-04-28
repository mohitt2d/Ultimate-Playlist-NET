#region Usings

using System;
using System.Globalization;
using AutoMapper;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.MobileApi.Models.Reward;
using UltimatePlaylist.MobileApi.Models.Ticket;
using UltimatePlaylist.Services.Common.Models.Reward;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class TicketProfile : Profile
    {
        #region Constructor(s)

        public TicketProfile()
        {
            // Request Model => Write Service Model
            CreateMap<AddTicketRequestModel, AddTicketWriteServiceModel>()
                .ForMember(s => s.EarnedType, o => o.MapFrom(t => Enum.Parse<TicketEarnedType>(t.EarnedType)))
                .ForMember(s => s.Type, o => o.MapFrom(t => Enum.Parse<TicketType>(t.Type)));

            // Read Service Model => Response Model
            CreateMap<TicketsStatsReadServiceModel, TicketsStatsResponseModel>();
            CreateMap<ActiveDrawingRewardReadServiceModel, ActiveDrawingRewardResponseModel>()
                .ForMember(s => s.Value, o => o.MapFrom(t => ConvertValueToString(t.Value)));
            CreateMap<CollectedDrawingRewardReadServiceModel, CollectedDrawingRewardResponseModel>()
                .ForMember(s => s.Value, o => o.MapFrom(t => ConvertValueToString(t.Value)))
                .ForMember(s => s.CollectedDate, o => o.MapFrom(t => ConvertDateToStringFormat(t.CollectedDate)));

            CreateMap<EarnedTicketsReadServiceModel, LastEarnedNotDisplayedTicketsResponseModel>();
        }

        #endregion

        #region Private Methods

        private string ConvertValueToString(decimal value)
        {
            var specifier = "C0";
            return value.ToString(specifier, CultureInfo.CreateSpecificCulture("en-US")).Replace(',', ' ');
        }

        private string ConvertDateToStringFormat(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
