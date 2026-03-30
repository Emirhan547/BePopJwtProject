using BePopJwt.Business.Dtos.AlbumDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.AlbumValidators
{
    public class CreateAlbumValidator:AbstractValidator<CreateAlbumDto>
    {
        public CreateAlbumValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Album name is required.");
            RuleFor(x => x.CoverUrl).NotEmpty().WithMessage("Cover URL is required.");
           

        }
    }
}
