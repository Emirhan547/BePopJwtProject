using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Validators.UserSongHistoryValidators
{
    public class CreateUserSongHistoryValidator:AbstractValidator<CreateUserSongHistoryDto>
    {
        public CreateUserSongHistoryValidator()
        {  
            RuleFor(x=>x.UserId).NotEmpty().WithMessage("UserId Boş Olamaz");
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId Boş Olamaz");
            RuleFor(x => x.PlayedAt).NotEmpty().WithMessage("PlayedAt Boş Olamaz");
            RuleFor(x => x.PlayDuration).NotEmpty().WithMessage("PlayDuration Boş Olamaz");
           
        }
    }
}
