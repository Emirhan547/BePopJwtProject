using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PlayerDtos;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using BePopJwt.DataAccess.Repositories.PlayerRepositories;
using BePopJwt.DataAccess.Repositories.SongRepositories;
using BePopJwt.DataAccess.Repositories.UserSongRepositories;
using BePopJwt.DataAccess.Uow;
using BePopJwt.Entity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BePopJwt.Business.Services.PlayerServices
{
    public class PlayerService(
        IPlayerRepository playerRepository,
        ISongRepository songRepository,
        IUserSongHistoryRepository userSongHistoryRepository,
        UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork) : IPlayerService
    {
        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetAccessibleSongsAsync(int userId)
        {
            var user = await userManager.Users
                .Include(x => x.Package)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Kullanıcı bulunamadı.");
            }

            var songs = await songRepository.GetSongsWithAlbumAsync();
            var accessibleSongs = songs
                .Where(x => (int)x.Level >= user.Package.Level)
                .ToList();

            return BaseResult<List<ResultSongWithAlbumDto>>.Success(accessibleSongs.Adapt<List<ResultSongWithAlbumDto>>());
        }

        public async Task<BaseResult<List<ResultUserSongHistoryWithDetailsDto>>> GetMyHistoryAsync(int userId)
        {
            var values = await userSongHistoryRepository.GetHistoriesWithSongAndUserAsync(userId);
            return BaseResult<List<ResultUserSongHistoryWithDetailsDto>>.Success(values.Adapt<List<ResultUserSongHistoryWithDetailsDto>>());
        }

        public async Task<BaseResult<ResultPlayerStateDto>> GetPlayerStateAsync(int userId)
        {
            var player = await playerRepository.GetByUserIdAsync(userId);
            if (player is null)
            {
                return BaseResult<ResultPlayerStateDto>.Success(new ResultPlayerStateDto
                {
                    UserId = userId,
                    IsPlaying = false,
                    CurrentPositionSeconds = 0
                });
            }

            return BaseResult<ResultPlayerStateDto>.Success(player.Adapt<ResultPlayerStateDto>());
        }

        public async Task<BaseResult<ResultUserSongHistoryDto>> PlaySongAsync(int userId, PlaySongRequestDto request)
        {
            var user = await userManager.Users
                .Include(x => x.Package)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail("Kullanıcı bulunamadı.");
            }

            var song = await songRepository.GetByIdAsync(request.SongId);
            if (song is null)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail("Şarkı bulunamadı.");
            }

            if ((int)song.Level < user.Package.Level)
            {
                return BaseResult<ResultUserSongHistoryDto>.Fail("Paketiniz bu şarkıyı dinlemek için yeterli değil.");
            }

            var history = new UserSongHistory
            {
                UserId = userId,
                SongId = request.SongId,
                PlayDuration = request.PlayDuration <= 0 ? song.Duration : request.PlayDuration,
                PlayedAt = DateTime.UtcNow
            };

            await userSongHistoryRepository.CreateAsync(history);

            var playerState = await playerRepository.GetByUserIdAsync(userId);
            if (playerState is null)
            {
                playerState = new Player
                {
                    UserId = userId,
                    SongId = request.SongId,
                    IsPlaying = true,
                    CurrentPositionSeconds = 0,
                    LastPlayedAt = DateTime.UtcNow
                };
                await playerRepository.CreateAsync(playerState);
            }
            else
            {
                playerState.SongId = request.SongId;
                playerState.IsPlaying = true;
                playerState.CurrentPositionSeconds = 0;
                playerState.LastPlayedAt = DateTime.UtcNow;
                playerRepository.Update(playerState);
            }

            var result = await unitOfWork.SaveChangesAsync();

            return result > 0
                ? BaseResult<ResultUserSongHistoryDto>.Success(history.Adapt<ResultUserSongHistoryDto>())
                : BaseResult<ResultUserSongHistoryDto>.Fail("Dinleme geçmişi kaydedilemedi.");
        }
    }
}