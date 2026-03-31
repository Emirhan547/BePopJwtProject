using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.BannerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.BannerServices
{
    public interface IBannerService
    {
        Task<BaseResult<List<ResultBannerDto>>> GetAllAsync();
        Task<BaseResult<ResultBannerDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultBannerDto>> CreateAsync(CreateBannerDto createBannerDto);
        Task<BaseResult<ResultBannerDto>> UpdateAsync(UpdateBannerDto updateBannerDto);
        Task<BaseResult<bool>>DeleteAsync(int id);
    }
}
