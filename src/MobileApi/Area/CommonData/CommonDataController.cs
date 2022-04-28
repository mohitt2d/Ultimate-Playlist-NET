#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.CommonData;
using UltimatePlaylist.Services.Common.Interfaces.CommonData;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.CommonData
{
    [Area("CommonData")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = MobileApiGroups.CommonData)]
    public class CommonDataController : BaseController
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IGenderService> GenderServiceProvider;

        #endregion

        #region Constructor(s)

        public CommonDataController(
            Lazy<IMapper> mapperProvider,
            Lazy<IGenderService> genderServiceProvider)
        {
            MapperProvider = mapperProvider;
            GenderServiceProvider = genderServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IGenderService GenderService => GenderServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("genders")]
        [ProducesEnvelope(typeof(IList<GenderResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGenders()
        {
            return await GenderService.GetGenders()
               .Map(result => Mapper.Map<IList<GenderResponseModel>>(result))
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}