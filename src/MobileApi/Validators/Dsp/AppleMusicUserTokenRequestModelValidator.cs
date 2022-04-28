#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Dsp.AppleMusic;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Dsp
{
    public class AppleMusicUserTokenRequestModelValidator : AbstractValidator<AppleMusicUserTokenRequestModel>
    {
        public AppleMusicUserTokenRequestModelValidator()
        {
            RuleFor(p => p.UserToken)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}
