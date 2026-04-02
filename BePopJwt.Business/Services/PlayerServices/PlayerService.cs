using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PlayerDtos;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using BePopJwt.Business.Services.OpenAiServices;
using BePopJwt.Business.Services.RecommendationServices;
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
        IRecommendationService recommendationService,
        IOpenAiMoodService openAiMoodService,
       IAppUserRepository appUserRepository,
        IUnitOfWork unitOfWork) : IPlayerService
    {
        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetAccessibleSongsAsync(int userId)
        {
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

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
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

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
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

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

            var recommendationIds = (await recommendationService.RecommendSongsForUserAsync(
                userId,
                accessibleSongs.Select(x => x.Id).ToList(),
                mySongIds,
                take)).ToList();

            if (recommendationIds.Count == 0)
            {
                recommendationIds = allHistories
                    .Where(x => similarUserIds.Contains(x.UserId) && !mySongIds.Contains(x.SongId))
                    .GroupBy(x => x.SongId)
                    .OrderByDescending(g => g.Count())
                    .ThenByDescending(g => g.Max(x => x.PlayedAt))
                    .Select(g => g.Key)
                    .Take(take)
                    .ToList();
            }

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
        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetMoodBasedRecommendationsAsync(int userId, string mood, int take = 6)
        {
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

            if (user is null)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Kullanıcı bulunamadı.");
            }

            var normalizedMood = (mood ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(normalizedMood))
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Ruh hali boş olamaz.");
            }

            var accessibleSongs = (await songRepository.GetSongsWithAlbumAsync())
                .Where(x => (int)x.Level >= user.Package.Level)
                .ToList();

            if (accessibleSongs.Count == 0)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Success([]);
            }

            var localMoodKeywords = new Dictionary<string, string[]>
            {
                ["mutlu"] = ["happy", "sun", "dance", "up", "fun", "light", "party", "good", "love", "joy"],
                ["enerjik"] = ["power", "run", "fire", "rise", "beat", "club", "dance", "drive", "fast", "wild"],
                ["sakin"] = ["calm", "night", "soft", "dream", "slow", "ocean", "moon", "chill", "blue", "peace"],
                ["hüzünlü"] = ["sad", "rain", "alone", "broken", "tears", "lost", "dark", "fall", "empty", "cry"],
                ["odak"] = ["focus", "ambient", "study", "piano", "lofi", "deep", "mind", "flow", "minimal", "cloud"]
            };

            var selectedKeywords = (await openAiMoodService.GetMoodKeywordsAsync(normalizedMood))
                .ToList();

            if (selectedKeywords.Count == 0)
            {
                selectedKeywords = localMoodKeywords
                .Where(x => normalizedMood.Contains(x.Key))
                .SelectMany(x => x.Value)
                .Distinct()
                .ToList();
            }

            if (selectedKeywords.Count == 0)
            {
                selectedKeywords = [normalizedMood];
            }

            var rankedSongs = await RankSongsByKeywordsAsync(accessibleSongs, selectedKeywords, Math.Clamp(take, 1, 20));

            return BaseResult<List<ResultSongWithAlbumDto>>.Success(rankedSongs.Adapt<List<ResultSongWithAlbumDto>>());
        }
        public async Task<BaseResult<List<ResultSongWithAlbumDto>>> GetPromptBasedRecommendationsAsync(int userId, string prompt, int take = 8)
        {
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

            if (user is null)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Kullanıcı bulunamadı.");
            }

            var normalizedPrompt = (prompt ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(normalizedPrompt))
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Fail("Prompt boş olamaz.");
            }

            var accessibleSongs = (await songRepository.GetSongsWithAlbumAsync())
                .Where(x => (int)x.Level >= user.Package.Level)
                .ToList();

            if (accessibleSongs.Count == 0)
            {
                return BaseResult<List<ResultSongWithAlbumDto>>.Success([]);
            }

            var selectedKeywords = (await openAiMoodService.GetPromptKeywordsAsync(normalizedPrompt))
                .ToList();
            if (selectedKeywords.Count == 0)
            {
                selectedKeywords = BuildPromptFallbackKeywords(normalizedPrompt).ToList();
            }

            if (selectedKeywords.Count == 0)
            {
                selectedKeywords = [normalizedPrompt];
            }

            var rankedSongs = await RankSongsByKeywordsAsync(accessibleSongs, selectedKeywords, Math.Clamp(take, 1, 20));

            return BaseResult<List<ResultSongWithAlbumDto>>.Success(rankedSongs.Adapt<List<ResultSongWithAlbumDto>>());
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
            var user = await appUserRepository.GetByIdWithPackageAsync(userId);

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
        private async Task<List<Song>> RankSongsByKeywordsAsync(List<Song> accessibleSongs, List<string> selectedKeywords, int take)
        {
            var histories = await userSongHistoryRepository.GetHistoriesWithSongAndUserAsync();
            var popularityLookup = histories
                .GroupBy(x => x.SongId)
                .ToDictionary(g => g.Key, g => g.Count());

            return accessibleSongs
                .Select(song =>
                {
                    var searchText = $"{song.Name} {song.Album?.Name}".ToLowerInvariant();
                    var moodScore = selectedKeywords.Count(k => searchText.Contains(k)) * 10;
                    var popularity = popularityLookup.TryGetValue(song.Id, out var count) ? count : 0;

                    return new
                    {
                        Song = song,
                        Score = moodScore + popularity
                    };
                })
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Song.Name)
                .Take(take)
                .Select(x => x.Song)
                .ToList();
        }

        private static IEnumerable<string> BuildPromptFallbackKeywords(string normalizedPrompt)
        {
            var map = new Dictionary<string, string[]>
            {
                ["mutlu"] = ["happy", "joy", "party", "light", "upbeat"],
                ["enerjik"] = ["power", "fire", "dance", "fast", "drive"],
                ["sakin"] = ["calm", "chill", "soft", "slow", "ambient"],
                ["hüzün"] = ["sad", "alone", "rain", "dark", "tears"],
                ["odak"] = ["focus", "study", "lofi", "minimal", "flow"],
                ["romantik"] = ["love", "heart", "sweet", "moon", "slow"],
                ["gece"] = ["night", "dark", "city", "neon", "moon"]
            };

            return map
                .Where(x => normalizedPrompt.Contains(x.Key))
                .SelectMany(x => x.Value)
                .Distinct();
        }
    }
}