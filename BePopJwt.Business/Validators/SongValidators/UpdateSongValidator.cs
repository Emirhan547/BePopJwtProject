using BePopJwt.Business.Dtos.SongDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.SongValidators
{
    public class UpdateSongValidator:AbstractValidator<UpdateSongDto>
    {
        public UpdateSongValidator()
        {
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Duration is required.");
            RuleFor(x => x.CoverUrl).NotEmpty().WithMessage("CoverUrl is required.");
        }
    }
}
