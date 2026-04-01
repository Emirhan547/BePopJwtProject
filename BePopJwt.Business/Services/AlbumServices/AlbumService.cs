using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.AlbumDtos;
using BePopJwt.DataAccess.Repositories.AlbumRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.AlbumServices
{
    public class AlbumService(IAlbumRepository _repository,IUnitOfWork _unitOfWork,IValidator<CreateAlbumDto> _createValidator,IValidator<UpdateAlbumDto>_updateValidator) : IAlbumService
    {
        public async Task<BaseResult<ResultAlbumDto>> CreateAsync(CreateAlbumDto createAlbumDto)
        {
            var validation= await _createValidator.ValidateAsync(createAlbumDto);
            if(!validation.IsValid)
            {
                return BaseResult<ResultAlbumDto>.Fail(validation.Errors);
            }

            var mappedAlbum = createAlbumDto.Adapt<Album>();
            await _repository.CreateAsync(mappedAlbum);
            var uow= await _unitOfWork.SaveChangesAsync();

            return uow > 0 ? BaseResult<ResultAlbumDto>.Success(mappedAlbum.Adapt<ResultAlbumDto>()) 
                : BaseResult<ResultAlbumDto>.Fail("Failed to update album.");

        }

        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
            var album = await _repository.GetByIdAsync(id);
            if(album is null)
            {
                return BaseResult<bool>.Fail("Album nulunamadı");
            }
            _repository.Delete(album);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<bool>.Fail("Album Silinemedi") : BaseResult<bool>.Success(true);
        }

        public async Task<BaseResult<List<ResultAlbumDto>>> GetAlbumsWithArtistAsync()
        {
            var albumsWithArtist= await _repository.GetAlbumsWithArtistAsync();
            var mapped=albumsWithArtist.Adapt<List<ResultAlbumDto>>();
            return BaseResult<List<ResultAlbumDto>>.Success(mapped);
        }

        public async Task<BaseResult<List<ResultAlbumDto>>> GetAllAsync()
        {
           var albums=await _repository.GetAllAsync();
            
            return BaseResult<List<ResultAlbumDto>>.Success(albums.Adapt<List<ResultAlbumDto>>());
        }

        public async Task<BaseResult<ResultAlbumDto>> GetByIdAsync(int id)
        {
            var album =await _repository.GetByIdAsync(id);
            if (album == null)
            {
                return BaseResult<ResultAlbumDto>.Fail("Album not found");
            }
            return BaseResult<ResultAlbumDto>.Success(album.Adapt<ResultAlbumDto>());
        }

        public async Task <BaseResult<ResultAlbumDto>>UpdateAsync(UpdateAlbumDto updateAlbumDto)
        {
            var validation=await _updateValidator.ValidateAsync(updateAlbumDto);
            if(!validation.IsValid)
            {
                return BaseResult<ResultAlbumDto>.Fail(validation.Errors);
            }
            var albums=await _repository.GetByIdAsync(updateAlbumDto.Id);
            if(albums == null)
            {
                return BaseResult<ResultAlbumDto>.Fail("Album Not Found");
            }
            var mappedAlbums=updateAlbumDto.Adapt(albums);
            _repository.Update(mappedAlbums);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<ResultAlbumDto>.Success(mappedAlbums.Adapt<ResultAlbumDto>())
                : BaseResult<ResultAlbumDto>.Fail("Failed to update album.");
        }
        
    }
}
