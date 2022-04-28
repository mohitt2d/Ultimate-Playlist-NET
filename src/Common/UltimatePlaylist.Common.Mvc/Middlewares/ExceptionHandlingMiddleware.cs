#region Usings

using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        #region Private Members

        private readonly RequestDelegate Next;
        private readonly IOptions<MvcNewtonsoftJsonOptions> JsonOptions;
        private readonly ILogger<ExceptionHandlingMiddleware> Logger;

        #endregion

        #region Constructor(s)

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
            ILoggerFactory loggerFactory)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
            JsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
            Logger = loggerFactory?.CreateLogger<ExceptionHandlingMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region Invoke

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception exception)
            {
                context.Response.Clear();

                var envelope = new EmptyEnvelope();

                switch (exception)
                {
                    case NotFoundException noe:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        envelope.Errors.Add(new Error(HttpStatusCode.NotFound.ToString(), noe.Message));
                        Logger.LogError($"Exception: {noe.Message}");
                        break;
                    case UnauthorizedAccessException una:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        envelope.Errors.Add(new Error(HttpStatusCode.Unauthorized.ToString(), una.Message));
                        Logger.LogError($"Exception: {una.Message}");
                        break;
                    case BadRequestException bad:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        envelope.Errors.Add(new Error(HttpStatusCode.BadRequest.ToString(), bad.Message));
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        envelope.Errors.Add(new Error(HttpStatusCode.InternalServerError.ToString(), exception.Message));
                        Logger.LogWarning($"Exception: {exception.Message}");
                        break;
                }

                context.Response.ContentType = MediaTypeNames.Application.Json;
                var json = JsonConvert.SerializeObject(envelope, JsonOptions.Value.SerializerSettings);

                await context.Response.WriteAsync(json);

                return;
            }
        }

        #endregion
    }
}
