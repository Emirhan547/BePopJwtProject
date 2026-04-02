using BePopJwt.DataAccess.Repositories.UserSongRepositories;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace BePopJwt.Business.Services.RecommendationServices
{
    public class MlNetRecommendationService(IUserSongHistoryRepository userSongHistoryRepository) : IRecommendationService
    {
        private readonly MLContext _mlContext = new(seed: 42);

        public async Task<IReadOnlyList<int>> RecommendSongsForUserAsync(
            int userId,
            IReadOnlyCollection<int> candidateSongIds,
            IReadOnlyCollection<int> excludedSongIds,
            int take)
        {
            if (candidateSongIds.Count == 0 || take <= 0)
            {
                return [];
            }

            var histories = await userSongHistoryRepository.GetHistoriesWithSongAndUserAsync();
            var grouped = histories
                .GroupBy(x => new { x.UserId, x.SongId })
                .Select(g => new SongInteractionData
                {
                    UserId = (uint)g.Key.UserId,
                    SongId = (uint)g.Key.SongId,
                    Label = g.Count()
                })
                .ToList();

            if (grouped.Count < 5)
            {
                return [];
            }

            var trainingData = _mlContext.Data.LoadFromEnumerable(grouped);
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = nameof(SongInteractionData.UserId),
                MatrixRowIndexColumnName = nameof(SongInteractionData.SongId),
                LabelColumnName = nameof(SongInteractionData.Label),
                NumberOfIterations = 25,
                ApproximationRank = 64,
                LearningRate = 0.1
            };

            var model = _mlContext.Recommendation().Trainers.MatrixFactorization(options).Fit(trainingData);
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SongInteractionData, SongScorePrediction>(model);

            var excluded = excludedSongIds.ToHashSet();

            return candidateSongIds
                .Where(songId => !excluded.Contains(songId))
                .Select(songId => new
                {
                    SongId = songId,
                    Score = predictionEngine.Predict(new SongInteractionData
                    {
                        UserId = (uint)userId,
                        SongId = (uint)songId
                    }).Score
                })
                .OrderByDescending(x => x.Score)
                .Take(take)
                .Select(x => x.SongId)
                .ToList();
        }

        private sealed class SongInteractionData
        {
            [KeyType(count: 100000)]
            public uint UserId { get; set; }

            [KeyType(count: 100000)]
            public uint SongId { get; set; }

            public float Label { get; set; }
        }

        private sealed class SongScorePrediction
        {
            public float Score { get; set; }
        }
    }
}