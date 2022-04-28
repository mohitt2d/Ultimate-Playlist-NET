#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Ticket;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Ticket
{
    public class AddTicketRequestValidator : AbstractValidator<AddTicketRequestModel>
    {
        public AddTicketRequestValidator()
        {
            RuleFor(p => p.ExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.EarnedType)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MustValueInEnum<AddTicketRequestModel, TicketEarnedType>();

            RuleFor(p => p.Type)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MustValueInEnum<AddTicketRequestModel, TicketType>();
        }
    }
}
