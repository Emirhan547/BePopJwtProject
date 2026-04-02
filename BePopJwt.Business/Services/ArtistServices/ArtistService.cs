using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.ArtistDtos;
using BePopJwt.DataAccess.Repositories.ArtistRepositories;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.ArtistServices
{
    public class ArtistService(IArtistRepository _artistRepository,IUnitOfWork _unitOfWork,IValidator<CreateArtistDto> _createValidator,IValidator<UpdateArtistDto> _updateValidator) : IArtistService
    {
        public async Task<BaseResult<ResultArtistDto>> CreateAsync(CreateArtistDto createArtistDto)
        {
            var validationArtist=await _createValidator.ValidateAsync(createArtistDto);
            if(!validationArtist.IsValid)
            {
                return BaseResult<ResultArtistDto>.Fail(validationArtist.Errors);
            }
            var mappedArtist = createArtistDto.Adapt<Artist>();
            await _artistRepository.CreateAsync(mappedArtist);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<ResultArtistDto>.Fail("Artist Eklenemedi") : BaseResult<ResultArtistDto>.Success(mappedArtist.Adapt<ResultArtistDto>());
        }

        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
           var artist=await _artistRepository.GetByIdAsync(id);
            if(artist is null)
            {
                return BaseResult<bool>.Fail("Artist Bulunamadı");
            }
             _artistRepository.Delete(artist);
           var uow= await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<bool>.Success(true) : BaseResult<bool>.Fail("Artist Silinemedi");
        }

        public async Task<BaseResult<List<ResultArtistDto>>> GetAllAsync()
        {
            var artist=await _artistRepository.GetAllAsync();
            var mappedArtist = artist.Adapt<List<ResultArtistDto>>();
            return BaseResult<List<ResultArtistDto>>.Success(mappedArtist);
        }

        public async Task<BaseResult<List<ResultArtistWithAlbumDto>>> GetArtistsWithAlbumsAsync()
        {
            var artistWithAlbums=await _artistRepository.GetArtistsWithAlbumsAsync();
            var mapped=artistWithAlbums.Adapt<List<ResultArtistWithAlbumDto>>();
            return BaseResult<List<ResultArtistWithAlbumDto>>.Success(mapped);
        }
       
        public async Task<BaseResult<ResultArtistDto>> GetByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist is null)
            {
                return BaseResult<ResultArtistDto>.Fail("Artist Bulunamdı");
            }
            var mappedArtist=artist.Adapt<ResultArtistDto>();
            return BaseResult<ResultArtistDto>.Success(mappedArtist);
        }

        public async Task<BaseResult<ResultArtistDto>> UpdateAsync(UpdateArtistDto updateArtistDto)
        {
            var validationArtist=await _updateValidator.ValidateAsync(updateArtistDto);
            if(!validationArtist.IsValid)
            {
                return BaseResult<ResultArtistDto>.Fail(validationArtist.Errors);
            }
            var artist = await _artistRepository.GetByIdAsync(updateArtistDto.Id);
            if(artist is null)
            {
                return BaseResult<ResultArtistDto>.Fail("Artist Bulunamadı");
            }
            var mapped = updateArtistDto.Adapt(artist);
            _artistRepository.Update(mapped);
           var uow= await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<ResultArtistDto>.Success(mapped.Adapt<ResultArtistDto>()) : BaseResult<ResultArtistDto>.Fail("Artist Güncellenemedi");
               

        }
    }
}
