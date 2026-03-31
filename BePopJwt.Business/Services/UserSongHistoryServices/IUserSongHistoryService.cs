using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.UserSongHistoryServices
{
    public interface IUserSongHistoryService
    {
        Task<BaseResult<List<ResultUserSongHistoryDto>>> GetAllAsync();
        Task<BaseResult<ResultUserSongHistoryDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultUserSongHistoryDto>> CreateAsync(CreateUserSongHistoryDto create);
        Task<BaseResult<ResultUserSongHistoryDto>>UpdateAsync(UpdateUserSongHistoryDto update);
        Task<BaseResult<bool>> DeleteAsync(int id);
    }
}
