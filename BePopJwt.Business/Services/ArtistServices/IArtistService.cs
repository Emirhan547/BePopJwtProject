using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.ArtistDtos;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.ArtistServices
{
    public interface IArtistService
    {
        Task<BaseResult<List<ResultArtistDto>>> GetAllAsync();
        Task<BaseResult<ResultArtistDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultArtistDto>> CreateAsync(CreateArtistDto createArtistDto);
        Task<BaseResult<ResultArtistDto>> UpdateAsync(UpdateArtistDto updateArtistDto);
        Task<BaseResult<bool>>DeleteAsync(int id);
        Task<BaseResult<List<ResultArtistWithAlbumDto>>> GetArtistsWithAlbumsAsync();
    }
}
