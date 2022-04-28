#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Filters
{
    public class ValidationFilter : IActionFilter
    {
        #region OnActionExecuting

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var envelope = new Envelope<EmptyData>();

            foreach (var invalidProperty in context.ModelState)
            {
                var errorMessage = string.Empty;
                foreach (var error in invalidProperty.Value.Errors)
                {
                    errorMessage += $"{error.ErrorMessage} ";
                }

                envelope.Errors.Add(new Error(invalidProperty.Key, errorMessage.Trim(' '), true));
            }

            context.Result = new BadRequestObjectResult(envelope);
        }

        #endregion

        #region OnActionExecuted

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var envelope = new Envelope<EmptyData>();

            foreach (var invalidProperty in context.ModelState)
            {
                var errorMessage = string.Empty;
                foreach (var error in invalidProperty.Value.Errors)
                {
                    errorMessage += $"{error.ErrorMessage} ";
                }

                envelope.Errors.Add(new Error(invalidProperty.Key, errorMessage.Trim(' '), true));
            }

            context.Result = new BadRequestObjectResult(envelope);
        }

        #endregion
    }
}