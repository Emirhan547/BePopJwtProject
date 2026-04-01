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
        public async Task<BaseResult<string>> GetSongSourceAsync(int userId, int songId)
        {
            var user = await userManager.Users
                .Include(x => x.Package)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult<string>.Fail("Kullanıcı bulunamadı.");
            }

            var song = await songRepository.GetByIdAsync(songId);
            if (song is null)
            {
                return BaseResult<string>.Fail("Şarkı bulunamadı.");
            }

            if ((int)song.Level < user.Package.Level)
            {
                return BaseResult<string>.Fail("Paketiniz bu şarkıyı dinlemek için yeterli değil.");
            }

            if (string.IsNullOrWhiteSpace(song.FilePath))
            {
                return BaseResult<string>.Fail("Şarkı dosya yolu bulunamadı.");
            }

            return BaseResult<string>.Success(song.FilePath);
        }
        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetRecommendationsAsync(int userId, int take = 6)
        {
            var user = await userManager.Users
                .Include(x => x.Package)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Kullanıcı bulunamadı.");
            }

            var accessibleSongs = (await songRepository.GetSongsWithAlbumAsync())
                .Where(x => (int)x.Level >= user.Package.Level)
                .ToList();

            if (accessibleSongs.Count == 0)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Success([]);
            }

            var allHistories = await userSongHistoryRepository.GetHistoriesWithSongAndUserAsync();
            var mySongIds = allHistories
                .Where(x => x.UserId == userId)
                .Select(x => x.SongId)
                .Distinct()
                .ToHashSet();

            if (mySongIds.Count == 0)
            {
                var coldStart = accessibleSongs
                    .OrderBy(x => x.Level)
                    .ThenBy(x => x.Name)
                    .Take(take)
                    .ToList();

                return BaseResult<List<ResultSongWithAlbumDto>>.Success(coldStart.Adapt<List<ResultSongWithAlbumDto>>());
            }

            var similarUserIds = allHistories
                .Where(x => mySongIds.Contains(x.SongId) && x.UserId != userId)
                .Select(x => x.UserId)
                .Distinct()
                .ToHashSet();

            var recommendationIds = allHistories
                .Where(x => similarUserIds.Contains(x.UserId) && !mySongIds.Contains(x.SongId))
                .GroupBy(x => x.SongId)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Max(x => x.PlayedAt))
                .Select(g => g.Key)
                .ToList();

            var recommendationSongs = accessibleSongs
                .Where(x => recommendationIds.Contains(x.Id))
                .OrderBy(x => recommendationIds.IndexOf(x.Id))
                .Take(take)
                .ToList();

            if (recommendationSongs.Count < take)
            {
                var fallback = accessibleSongs
                    .Where(x => !mySongIds.Contains(x.Id) && recommendationSongs.All(r => r.Id != x.Id))
                    .OrderBy(x => x.Level)
                    .ThenBy(x => x.Name)
                    .Take(take - recommendationSongs.Count)
                    .ToList();

                recommendationSongs.AddRange(fallback);
            }

            return BaseResult<List<ResultSongWithAlbumDto>>.Success(recommendationSongs.Adapt<List<ResultSongWithAlbumDto>>());
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