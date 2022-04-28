#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Schedule
{
    public class CalendarDataRequestValidator : AbstractValidator<CalendarDataRequestModel>
    {
        public CalendarDataRequestValidator()
        {
            RuleFor(p => p.CalendarDateTimeStamp)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}
