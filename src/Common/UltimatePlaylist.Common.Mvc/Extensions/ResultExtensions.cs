#region Usings

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class ResultExtensions
    {
        public static Envelope<EmptyData> ToEnvelope(this Result result)
        {
            return result.Map(() => default(EmptyData)).ToEnvelope();
        }

        public static Envelope<T> ToEnvelope<T>(this Result<T> result)
        {
            var envelope = new Envelope<T>()
            {
                Errors = new List<Error>(),
            };

            if (result.IsSuccess)
            {
                envelope.Data = result.Value;
            }
            else
            {
                envelope.Errors.Add(new Error(result.GetStatusCodeString(), result.Error));
            }

            return envelope;
        }

        public static Envelope<T> ToEnvelope<T>(this Result<T, Error> result)
        {
            var envelope = new Envelope<T>()
            {
                Errors = new List<Error>(),
            };

            if (result.IsSuccess)
            {
                envelope.Data = result.Value;
            }
            else
            {
                envelope.Errors.Add(result.Error);
            }

            return envelope;
        }

        public static int GetStatusCode(this Result result)
        {
            if (result.IsSuccess)
            {
                return StatusCodes.Status200OK;
            }

            if (result.Error.EndsWith(nameof(ErrorType.NotFound)))
            {
                return StatusCodes.Status404NotFound;
            }

            return result.Error switch
            {
                nameof(ErrorType.Unauthenticated) => StatusCodes.Status401Unauthorized,
                nameof(ErrorType.Forbidden) => StatusCodes.Status403Forbidden,
                nameof(ErrorType.Unexpected) => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest,
            };
        }

        public static int GetStatusCode<T>(this Result<T, Error> result)
        {
            if (result.IsSuccess)
            {
                return StatusCodes.Status200OK;
            }

            if (result.Error.Code.EndsWith(nameof(ErrorType.NotFound)))
            {
                return StatusCodes.Status404NotFound;
            }

            return result.Error.Code switch
            {
                nameof(ErrorType.Unauthenticated) => StatusCodes.Status401Unauthorized,
                nameof(ErrorType.Forbidden) => StatusCodes.Status403Forbidden,
                nameof(ErrorType.Unexpected) => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest,
            };
        }

        public static int GetStatusCode<T>(this Result<T> result)
        {
            return ((Result)result).GetStatusCode();
        }

        public static string GetStatusCodeString<T>(this Result<T> result)
        {
            return ((Result)result).GetStatusCodeString();
        }

        public static string GetStatusCodeString(this Result result)
        {
            if (result.IsSuccess)
            {
                return HttpStatusCode.OK.ToString();
            }

            if (result.Error.EndsWith(nameof(ErrorType.NotFound)))
            {
                return HttpStatusCode.NotFound.ToString();
            }

            return result.Error switch
            {
                nameof(ErrorType.Unauthenticated) => HttpStatusCode.Unauthorized.ToString(),
                nameof(ErrorType.Forbidden) => HttpStatusCode.Forbidden.ToString(),
                nameof(ErrorType.Unexpected) => HttpStatusCode.InternalServerError.ToString(),
                _ => HttpStatusCode.BadRequest.ToString(),
            };
        }

        public static Task<Result<TResult>> BindIf<TCurrent, TResult>(
            this Result<TCurrent> currentResult,
            Func<Result<TCurrent>, bool> condition,
            Func<Task<Result<TResult>>> actionIfTrue,
            Func<Task<Result<TResult>>> actionIfFalse)
        {
            return condition(currentResult)
                ? actionIfTrue()
                : actionIfFalse();
        }
    }
}
