using BePopJwt.Business.Dtos.PackageDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.PackageValidators
{
    public class CreatePackageValidator:AbstractValidator<CreatePackageDto>
    {
        public CreatePackageValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş olamaz.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name alanı boş olamaz.");
            RuleFor(x => x.Level).NotEmpty().WithMessage("Level alanı boş olamaz.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price alanı boş olamaz.");
        }
    }
}
