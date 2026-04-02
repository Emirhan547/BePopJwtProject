using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.OpenAiServices
{
    public interface IOpenAiMoodService
    {
        Task<IReadOnlyList<string>> GetMoodKeywordsAsync(string mood, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<string>> GetPromptKeywordsAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
