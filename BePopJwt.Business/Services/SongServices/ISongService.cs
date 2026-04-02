using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.SongDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.SongServices
{
    public interface ISongService
    {
        Task<BaseResult<List<ResultSongDto>>> GetAllAsync();
        Task<BaseResult<List<ResultSongWithAlbumDto>>> GetSongsWithAlbumAsync();
        Task<BaseResult<ResultSongDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultSongWithAlbumDto>> GetSongWithAlbumByIdAsync(int id);
        Task<BaseResult<ResultSongDto>> CreateAsync(CreateSongDto createSongDto);
        Task<BaseResult<ResultSongDto>> UpdateAsync(UpdateSongDto updateSongDto);
        Task<BaseResult<bool>> DeleteAsync(int id);


    }
}
