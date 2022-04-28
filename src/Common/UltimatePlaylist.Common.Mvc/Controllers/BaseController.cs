#region Usings

using System;
using System.Linq;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.Common.Mvc.Models;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.Common.Mvc.Controllers
{
    [ApiController]
    [ProducesEmptyEnvelope(StatusCodes.Status400BadRequest)]
    [InTransaction]
    public class BaseController : ControllerBase
    {
        #region Constructor(s)

        public BaseController()
        {
        }

        #endregion

        #region Protected Properties

        protected Pagination XPagination => GetPaging();

        protected Guid XUserExternalId => GetUserExternalId();

        protected string XUserEmail => GetUserEmail();

        protected string XToken => GetToken();

        #endregion

        #region Private Properties

        private string PageSize => HttpContext?.Request.Headers[XParameters.PageSize].FirstOrDefault() ?? string.Empty;

        private string PageNumber => HttpContext?.Request.Headers[XParameters.PageNumber].FirstOrDefault() ?? string.Empty;

        private string Query => HttpContext?.Request.Headers[XParameters.Query].FirstOrDefault() ?? string.Empty;

        private string OrderBy => HttpContext?.Request.Headers[XParameters.Order].FirstOrDefault() ?? string.Empty;

        private string Descending => HttpContext?.Request.Headers[XParameters.Desc].FirstOrDefault() ?? "true";

        #endregion

        #region Protected Methods

        protected IActionResult BuildEnvelopeResult(int statusCode)
        {
            return BuildEnvelopeResult<EmptyData>(statusCode, data: null);
        }

        protected IActionResult BuildEnvelopeResult<T>(int statusCode, T data)
            where T : class
        {
            var envelope = new Envelope<T>(data);

            return StatusCode(statusCode, envelope);
        }

        protected IActionResult BuildEnvelopeResult(Result result)
        {
            var envelope = result.ToEnvelope();
            var statusCode = result.GetStatusCode();

            return StatusCode(statusCode, envelope);
        }

        protected IActionResult BuildEnvelopeResult<T>(Result<T> result)
        {
            var envelope = result.ToEnvelope();
            var statusCode = result.GetStatusCode();

            return StatusCode(statusCode, envelope);
        }

        protected IActionResult BuildEnvelopeResult<T>(Result<T, Error> result)
        {
            var envelope = result.ToEnvelope();
            var statusCode = result.GetStatusCode();

            return StatusCode(statusCode, envelope);
        }

        protected IActionResult BuildFileResult(Result<byte[]> result, string filename = null)
        {
            if (result.IsFailure)
            {
                return BuildEnvelopeResult(result);
            }

            return File(result.Value, "application/octet-stream", filename ?? $"file.xlsx");
        }

        #endregion

        #region Private Methods

        private Pagination GetPaging()
        {
            int.TryParse(PageSize, out int pageSize);
            int.TryParse(PageNumber, out int pageNumber);
            bool.TryParse(Descending, out bool desc);

            return new Pagination(pageSize, pageNumber, Query, OrderBy, desc);
        }

        private Guid GetUserExternalId()
        {
            if (Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaims.ExternalId))?.Value, out Guid externalId))
            {
                return externalId.Decode(XUserEmail);
            }

            throw new NotFoundException("UserId Not Found");
        }

        private string GetUserEmail()
        {
            return User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaims.Email))?.Value;
        }

        private string GetToken()
        {
            var token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                throw new NotFoundException("Token not found");
            }

            return token.Replace("Bearer ", string.Empty);
        }

        #endregion
    }
}