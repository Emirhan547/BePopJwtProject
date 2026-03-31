using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using BePopJwt.DataAccess.Repositories.UserSongRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.UserSongHistoryServices
{
    public class UserSongHistoryService(IUserSongHistoryRepository _repository,IValidator<CreateUserSongHistoryDto> _createValidator,
                                       IValidator<UpdateUserSongHistoryDto>_updateValidator,IUnitOfWork _unitOfWork) : IUserSongHistoryService
    {
        public async Task<BaseResult<ResultUserSongHistoryDto>> CreateAsync(CreateUserSongHistoryDto create)
        {
            var validations=await _createValidator.ValidateAsync(create);
            if(!validations.IsValid)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail(validations.Errors);
            }
            var mapped = create.Adapt<UserSongHistory>();
            await _repository.CreateAsync(mapped);
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<ResultUserSongHistoryDto>.Fail("UserSong eklenemedi") : BaseResult<ResultUserSongHistoryDto>.Success(mapped.Adapt<ResultUserSongHistoryDto>());
        }
        public async Task<BaseResult<List<ResultUserSongHistoryWithDetailsDto>>> GetAllWithSongAndUserAsync()
        {
            var values = await _repository.GetHistoriesWithSongAndUserAsync();
            return BaseResult<List<ResultUserSongHistoryWithDetailsDto>>.Success(values.Adapt<List<ResultUserSongHistoryWithDetailsDto>>());
        }
        public async Task<BaseResult<ResultUserSongHistoryWithDetailsDto>> GetByIdWithSongAndUserAsync(int id)
        {
            var values = await _repository.GetHistoryWithSongAndUserByIdAsync(id);
            if (values is null)
            {
                return BaseResult<ResultUserSongHistoryWithDetailsDto>.Fail("UserSong Bulunamadı");
            }

            return BaseResult<ResultUserSongHistoryWithDetailsDto>.Success(values.Adapt<ResultUserSongHistoryWithDetailsDto>());
        }
        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            if(values is null)
            {
                return BaseResult<bool>.Fail("UserSong bulunamadı");
            }
            _repository.Delete(values);
            var uow= await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<bool>.Fail("UserSong Silinemedi") : BaseResult<bool>.Success(true);
        }

        public async Task<BaseResult<List<ResultUserSongHistoryDto>>> GetAllAsync()
        {
            var values=await _repository.GetAllAsync();
            var mapped=values.Adapt<List<ResultUserSongHistoryDto>>();
            return BaseResult<List<ResultUserSongHistoryDto>>.Success(mapped);
        }

        public async Task<BaseResult<ResultUserSongHistoryDto>> GetByIdAsync(int id)
        {
            var values=await _repository.GetByIdAsync(id);
            if( values is null)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail("UserSong Bulunamadı");
            }
            var mapped = values.Adapt<ResultUserSongHistoryDto>();
            return BaseResult<ResultUserSongHistoryDto>.Success(mapped);
        }

        public async Task<BaseResult<ResultUserSongHistoryDto>> UpdateAsync(UpdateUserSongHistoryDto update)
        {
            var validation=await _updateValidator.ValidateAsync(update);
            if(!validation.IsValid)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail(validation.Errors);
            }
            var values=await _repository.GetByIdAsync(update.Id);
            if( values is null)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail("UserSong Bulunamadı");
            }
            var mapped=values.Adapt<UserSongHistory>();
            _repository.Update(mapped);
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<ResultUserSongHistoryDto>.Fail("UserSong Güncellenemedi") : BaseResult<ResultUserSongHistoryDto>.Success(mapped.Adapt<ResultUserSongHistoryDto>());
        }
    }
}
