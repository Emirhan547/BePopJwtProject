using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.BannerDtos;
using BePopJwt.DataAccess.Repositories.BannerRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.BannerServices
{
    public class BannerService(IBannerRepository _bannerRepository, IUnitOfWork _unitOfWork, IValidator<CreateBannerDto> _createValidator, IValidator<UpdateBannerDto> _updateValidator) : IBannerService
    {
        public async Task<BaseResult<ResultBannerDto>> CreateAsync(CreateBannerDto createBannerDto)
        {
            var validation = await _createValidator.ValidateAsync(createBannerDto);
            if (!validation.IsValid)
            {
                return BaseResult<ResultBannerDto>.Fail(validation.Errors);
            }
            var mapped = createBannerDto.Adapt<Banner>();
            await _bannerRepository.CreateAsync(mapped);
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<ResultBannerDto>.Success(mapped.Adapt<ResultBannerDto>()) : BaseResult<ResultBannerDto>.Fail("Banner Eklenemedi");
        }

        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
            var banners = await _bannerRepository.GetByIdAsync(id);
            if (banners == null)
            {
                return BaseResult<bool>.Fail("Banner Bulunamadı");
            }
            _bannerRepository.Delete(banners);
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<bool>.Success(true) : BaseResult<bool>.Fail("Banner Silinemedi");
        }

        public async Task<BaseResult<List<ResultBannerDto>>> GetAllAsync()
        {
            var banners = await _bannerRepository.GetAllAsync();
            var mapped = banners.Adapt<List<ResultBannerDto>>();
            return BaseResult<List<ResultBannerDto>>.Success(mapped);
        }

        public async Task<BaseResult<ResultBannerDto>> GetByIdAsync(int id)
        {
            var banners = await _bannerRepository.GetByIdAsync(id);
            if (banners is null)
            {
                return BaseResult<ResultBannerDto>.Fail("Banner Bulunamadı");
            }
            var mapped = banners.Adapt<ResultBannerDto>();
            return BaseResult<ResultBannerDto>.Success(mapped);
        }

        public async Task<BaseResult<ResultBannerDto>> UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            var validation = await _updateValidator.ValidateAsync(updateBannerDto);
            if (validation.IsValid)
            {
                return BaseResult<ResultBannerDto>.Fail(validation.Errors);
            }
            var banners = await _bannerRepository.GetByIdAsync(updateBannerDto.Id);
            if (banners is null)
            {
                return BaseResult<ResultBannerDto>.Fail("Banner Bulunamadı");
            }
            var mapped = updateBannerDto.Adapt(banners);
            _bannerRepository.Update(mapped);
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<ResultBannerDto>.Fail("Banner Güncellenemedi") : BaseResult<ResultBannerDto>.Success(mapped.Adapt<ResultBannerDto>());
        }
    }
}
