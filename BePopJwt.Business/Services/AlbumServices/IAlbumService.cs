using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.AlbumDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.AlbumServices
{
    public interface IAlbumService
    {
        Task<BaseResult<List<ResultAlbumDto>>> GetAllAsync();
        Task<BaseResult<ResultAlbumDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultAlbumDto>> CreateAsync(CreateAlbumDto createAlbumDto);
        Task<BaseResult<ResultAlbumDto>> UpdateAsync(UpdateAlbumDto updateAlbumDto);
        Task<BaseResult<bool>>DeleteAsync(int id);
        Task<BaseResult<List<ResultAlbumDto>>> GetAllWithDetailsAsync();
    }
}
