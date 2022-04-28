#region Usings

using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Primitives;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventHeadersValidatorService
    {
        Result Validate(IDictionary<string, StringValues> headers);
    }
}
