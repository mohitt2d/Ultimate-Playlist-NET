#region Usings

using System;
using System.Collections.Generic;
using AutoMapper;
using UltimatePlaylist.MobileApi.Models.Analytics;
using UltimatePlaylist.Services.Common.Models.Analytics;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class AnalyticsProfile : Profile
    {
        #region Constructor(s)

        public AnalyticsProfile()
        {
            // Request model => Write Service Mdoel
            CreateMap<SaveAnalyticsDataRequestModel, SaveAnalyticsDataWriteServiceModel>()
                .ForMember(s => s.PlaylistExternalId, o => o.MapFrom(t => MapStringToGuid(t.Params, "PlaylistExternalId")))
                .ForMember(s => s.SongExternalId, o => o.MapFrom(t => MapStringToGuid(t.Params, "SongExternalId")))
                .ForMember(s => s.ActualListeningSecond, o => o.MapFrom(t => MapStringToNullableInt(t.Params, "ActualListeningSecond")));

            // Read Service Model => Response Model
            CreateMap<AnalyticsLastEarnedTicketsReadServiceModel, AnalyticsLastEarnedTicketsResponseModel>();
        }

        #endregion

        #region Private Methods

        private Guid MapStringToGuid(Dictionary<string, string> dictionary, string searchedValue)
        {
            dictionary.TryGetValue(searchedValue, out string value);

            return Guid.Parse(value);
        }

        private int? MapStringToNullableInt(Dictionary<string, string> dictionary, string searchedValue)
        {
            dictionary.TryGetValue(searchedValue, out string value);

            return int.TryParse(value, out int result)
                ? result
                : null;
        }

        #endregion
    }
}
