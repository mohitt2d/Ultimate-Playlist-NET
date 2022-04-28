#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.MobileApi.Models.Notification;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Notification
{
    public class DeviceTokenRequestModelValidator : AbstractValidator<DeviceTokenRequestModel>
    {
        public DeviceTokenRequestModelValidator()
        {
            RuleFor(p => p.DeviceToken)
                .NotEmpty()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(ValidationErrorType.DeviceTokenEmpty.ToString());
        }
    }
}
