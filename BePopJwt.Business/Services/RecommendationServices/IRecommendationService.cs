using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.RecommendationServices
{
    public interface IRecommendationService
    {
        Task<IReadOnlyList<int>> RecommendSongsForUserAsync(
            int userId,
            IReadOnlyCollection<int> candidateSongIds,
            IReadOnlyCollection<int> excludedSongIds,
            int take);
    }
}
