#region Usings

using FluentValidation;
using UltimatePlaylist.Games.Models.Lottery;

#endregion

namespace UltimatePlaylist.Services.Games.Validators
{
    public class UltimatePayoutGameValidator : AbstractValidator<LotteryWinningNumbersReadServiceModel>
    {
        #region Public Methods

        public UltimatePayoutGameValidator()
        {
            RuleFor(p => p.FirstNumber)
                .NotEmpty()
                .InclusiveBetween(1, 45);

            RuleFor(p => p.SecondNumber)
             .NotEmpty()
             .InclusiveBetween(1, 45);

            RuleFor(p => p.ThirdNumber)
             .NotEmpty()
             .InclusiveBetween(1, 45);

            RuleFor(p => p.FourthNumber)
             .NotEmpty()
             .InclusiveBetween(1, 45);

            RuleFor(p => p.FifthNumber)
             .NotEmpty()
             .InclusiveBetween(1, 45);

            RuleFor(p => p.SixthNumber)
             .NotEmpty()
             .InclusiveBetween(1, 10);
        }

        #endregion
    }
}