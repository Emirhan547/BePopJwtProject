using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PlayerDtos;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.PlayerServices
{
    public interface IPlayerService
    {
        Task<BaseResult<List<ResultSongWithAlbumDto>>> GetAccessibleSongsAsync(int userId);
        Task<BaseResult<List<ResultSongWithAlbumDto>>> GetRecommendationsAsync(int userId, int take = 6);
        Task<BaseResult<ResultUserSongHistoryDto>> PlaySongAsync(int userId, PlaySongRequestDto request);
        Task<BaseResult<List<ResultUserSongHistoryWithDetailsDto>>> GetMyHistoryAsync(int userId);
        Task<BaseResult<ResultPlayerStateDto>> GetPlayerStateAsync(int userId);
    }
}
