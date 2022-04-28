#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Games;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Games
{
    public class UltimatePayoutRequestModelValidator : AbstractValidator<UltimatePayoutRequestModel>
    {
        #region Public Methods

        public UltimatePayoutRequestModelValidator()
        {
            RuleFor(p => p.UltimatePayoutNumbers)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => x.Count == 6)
                .WithMessage(ValidationMessages.HaveToBeSixElements)
                .ForEach(p => p.InclusiveBetween(1, 45))
                .WithMessage(ValidationMessages.InvalidUltimateValue);
        }

        #endregion
    }
}