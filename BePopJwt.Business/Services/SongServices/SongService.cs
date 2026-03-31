using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.DataAccess.Repositories.SongRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.SongServices
{
    public class SongService(ISongRepository _repository,IValidator<CreateSongDto> _creaeteValidator,
                             IValidator<UpdateSongDto>_updateValidator,IUnitOfWork _unitOfWork) : ISongService
    {
        public async Task<BaseResult<ResultSongDto>> CreateAsync(CreateSongDto createSongDto)
        {
            var validation=await _creaeteValidator.ValidateAsync(createSongDto);
            if (!validation.IsValid)
            {
                return BaseResult<ResultSongDto>.Fail(validation.Errors);
            }
            var mapped = createSongDto.Adapt<Song>();
            await _unitOfWork.SaveChangesAsync();
            var uow = await _unitOfWork.SaveChangesAsync();
            return uow < 0 ? BaseResult<ResultSongDto>.Fail("Song Eklenemedi") : BaseResult<ResultSongDto>.Success(mapped.Adapt<ResultSongDto>());
        }

        public async Task<BaseResult<bool>> DeleteAsync(int id)
        {
            var songs = await _repository.GetByIdAsync(id);
            if(songs is null)
            {
                return BaseResult<bool>.Fail("Song Bulunamadı");
            }
            _repository.Delete(songs);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow > 0 ? BaseResult<bool>.Success(true) : BaseResult<bool>.Fail("Song Silinemedi");
        }
        public async Task<BaseResult<ResultSongWithAlbumDto>> GetSongWithAlbumByIdAsync(int id)
        {
            var song = await _repository.GetSongWithAlbumByIdAsync(id);
            if (song is null)
            {
                return BaseResult<ResultSongWithAlbumDto>.Fail("Songs Bulunamadı");
            }

            return BaseResult<ResultSongWithAlbumDto>.Success(song.Adapt<ResultSongWithAlbumDto>());
        }

        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetSongsWithAlbumAsync()
        {
            var songs = await _repository.GetSongsWithAlbumAsync();
            return BaseResult<List<ResultSongWithAlbumDto>>.Success(songs.Adapt<List<ResultSongWithAlbumDto>>());
        }
        public async Task<BaseResult<List<ResultSongDto>>> GetAllAsync()
        {
            var songs=await _repository.GetAllAsync();
            var mapped = songs.Adapt<List<ResultSongDto>>();
            return BaseResult<List<ResultSongDto>>.Success(mapped);
        }

        public async Task<BaseResult<ResultSongDto>> GetByIdAsync(int id)
        {
            var songs = await _repository.GetByIdAsync(id);
            if(songs is null)
            {
                return BaseResult<ResultSongDto>.Fail("Songs Bulunamadı");
            }
            var mapped = songs.Adapt<ResultSongDto>();
            return BaseResult<ResultSongDto>.Success(mapped);
        }

        public async Task<BaseResult<ResultSongDto>> UpdateAsync(UpdateSongDto updateSongDto)
        {
            var validation=await _updateValidator.ValidateAsync(updateSongDto);
            if(!validation.IsValid)
            {
                return BaseResult<ResultSongDto>.Fail(validation.Errors);
            }
            var songs = await _repository.GetByIdAsync(updateSongDto.Id);
            if(songs is null)
            {
                return BaseResult<ResultSongDto>.Fail("Songs Bulunamadı");
            }
            var mapped = songs.Adapt(songs);
            _repository.Update(mapped);
            var uow=await _unitOfWork.SaveChangesAsync();
            return uow<0?BaseResult<ResultSongDto>.Fail("Songs Güncellenemedi"):BaseResult<ResultSongDto>.Success(mapped.Adapt<ResultSongDto>());

        }
        

    }
}
