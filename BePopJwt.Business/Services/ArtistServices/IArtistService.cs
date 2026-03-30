using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.ArtistDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.ArtistServices
{
    public interface IArtistService
    {
        Task<BaseResult<List<ResultArtistDto>>> GetAllAsync();
        Task<BaseResult<ResultArtistDto>> GetByIdAsync(int id);
        Task<BaseResult<CreateArtistDto>> CreateAsync(CreateArtistDto createArtistDto);
        Task<BaseResult<UpdateArtistDto>> UpdateAsync(UpdateArtistDto updateArtistDto);
        Task<BaseResult<bool>>DeleteAsync(int id);
        
    }
}
