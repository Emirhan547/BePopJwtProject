using BePopJwt.Business.Dtos.SongDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.SongValidators
{
    public class CreateSongValidator:AbstractValidator<CreateSongDto>
    {
        public CreateSongValidator()
        {
            RuleFor(x => x.FilePath).NotEmpty().WithMessage("FilePath is required.");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Duration is required.");
            RuleFor(x => x.CoverUrl).NotEmpty().WithMessage("CoverUrl is required.");
            RuleFor(x => x.Level).NotEmpty().WithMessage("Level is required.");
        }
    }
}
