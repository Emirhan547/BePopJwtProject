using BePopJwt.Business.Dtos.BannerDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.BannerValidators
{
    public class UpdateBannerValidator:AbstractValidator<UpdateBannerDto>
    {
        public UpdateBannerValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Subtitle).NotEmpty().WithMessage("Subtitle is required.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required.");
            RuleFor(x => x.SongId).GreaterThan(0).WithMessage("SongId must be greater than 0.");
        }
    }
}
