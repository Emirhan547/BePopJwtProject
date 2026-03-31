using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PackageDtos;
using BePopJwt.DataAccess.Repositories.PackageRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.PackageServices
{
    public class PackageService(
        IPackageRepository _repository,IUnitOfWork _unitOfWork,IValidator<CreatePackageDto>_createValidator,IValidator<UpdatePackageDto>_updateValidator) : IPackageService
    {
        public async Task<BaseResult<ResultPackageDto>> CreateAsync(CreatePackageDto createPackageDto)
        {
            var validation=await _createValidator.ValidateAsync(createPackageDto);
            if(!validation.IsValid)
            {
                return BaseResult<ResultPackageDto>.Fail(validation.Errors);
            }
            var mapped=createPackageDto.Adapt<Package>();
            await _repository.CreateAsync(mapped);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<ResultPackageDto>.Success(mapped.Adapt<ResultPackageDto>()) : BaseResult<ResultPackageDto>.Fail("Package Eklenemedi");
        }

        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
            var package = await _repository.GetByIdAsync(id);
            if(package is null)
            {
                return BaseResult<bool>.Fail("Package Bulunamadı");

            }
            _repository.Delete(package);
            var uow= await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<bool>.Fail("Package Silinemedi") : BaseResult<bool>.Success(true);
        }

        public async Task<BaseResult<List<ResultPackageDto>>> GetAllAsync()
        {
            var package=await _repository.GetAllAsync();
            var mapped=package.Adapt<List<ResultPackageDto>>();
            return BaseResult<List<ResultPackageDto>>.Success(mapped);
        }

        public async Task<BaseResult<ResultPackageDto>> GetByIdAsync(int id)
        {
            var package=await _repository.GetByIdAsync(id);
            if( package is null)
            {
                return BaseResult<ResultPackageDto>.Fail("Package Bulunamadı");
            }
            var mapped = package.Adapt<ResultPackageDto>();
            return BaseResult<ResultPackageDto>.Success(mapped);
        }
        public async Task<BaseResult<ResultPackageWithUsersDto>> GetPackageWithUsersByIdAsync(int id)
        {
            var package = await _repository.GetPackageWithUsersByIdAsync(id);
            if (package is null)
            {
                return BaseResult<ResultPackageWithUsersDto>.Fail("Package Bulunamadı");
            }

            return BaseResult<ResultPackageWithUsersDto>.Success(package.Adapt<ResultPackageWithUsersDto>());
        }

        public async Task<BaseResult<List<ResultPackageWithUsersDto>>> GetPackagesWithUsersAsync()
        {
            var packages = await _repository.GetPackagesWithUsersAsync();
            return BaseResult<List<ResultPackageWithUsersDto>>.Success(packages.Adapt<List<ResultPackageWithUsersDto>>());
        }
        public async Task<BaseResult<ResultPackageDto>> UpdateAsync(UpdatePackageDto updatePackageDto)
        {
            var validation = await _updateValidator.ValidateAsync(updatePackageDto);
            if(!validation.IsValid)
            {
                return BaseResult<ResultPackageDto>.Fail(validation.Errors);
            }
            var package=await _repository.GetByIdAsync(updatePackageDto.Id);
            {
                if(package is null)
                {
                    return BaseResult<ResultPackageDto>.Fail("Package Bulunamadı");
                }
                var mapped = updatePackageDto.Adapt(package);
                _repository.Update(mapped);
                var uow=await _unitOfWork.SaveChangesAsync();
                return uow<0?BaseResult<ResultPackageDto>.Fail("Package Güncellenemedi"):BaseResult<ResultPackageDto>.Success(mapped.Adapt<ResultPackageDto>());
            }
        }
    }
}
