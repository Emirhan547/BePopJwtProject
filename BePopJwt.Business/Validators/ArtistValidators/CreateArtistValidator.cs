using BePopJwt.Business.Dtos.ArtistDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.ArtistValidators
{
    public class CreateArtistValidator:AbstractValidator<CreateArtistDto>
    {
        public CreateArtistValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Artist name is required.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Artist image URL is required.");
            RuleFor(x => x.Bio).NotEmpty().WithMessage("Artist image URL is required.");
            RuleFor(x => x.CoverUrl).NotEmpty().WithMessage("Artist image URL is required.");

        }
    }
}
