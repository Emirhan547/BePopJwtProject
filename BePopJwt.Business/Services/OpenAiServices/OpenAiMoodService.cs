using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace BePopJwt.Business.Services.OpenAiServices
{
    public class OpenAiMoodService(HttpClient httpClient, IConfiguration configuration) : IOpenAiMoodService
    {
        public async Task<IReadOnlyList<string>> GetMoodKeywordsAsync(string mood, CancellationToken cancellationToken = default)
        {
            return await GetKeywordsInternalAsync(
                $"Ruh hali: {mood}. Müzik önerisi için en fazla 8 İngilizce anahtar kelime üret. Sadece virgülle ayrılmış liste döndür.",
                cancellationToken);
        }

        public async Task<IReadOnlyList<string>> GetPromptKeywordsAsync(string prompt, CancellationToken cancellationToken = default)
        {
            return await GetKeywordsInternalAsync(
                $"Kullanıcı promptu: {prompt}. Bu prompta göre müzik önerisi yapmak için en fazla 12 İngilizce anahtar kelime üret. İlk kelime mutlaka hissi/moodu anlatsın. Sadece virgülle ayrılmış liste döndür.",
                cancellationToken);
        }

        private async Task<IReadOnlyList<string>> GetKeywordsInternalAsync(string input, CancellationToken cancellationToken)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(input))
                {
                    return [];
                }

            var model = configuration["OpenAI:Model"] ?? "gpt-4.1-mini";
            
            var requestBody = new { model, input };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var response = await httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var document = JsonDocument.Parse(json);
            var text = ExtractOutputText(document.RootElement);

           
                if (string.IsNullOrWhiteSpace(text))
                {
                    return [];
                }

           
                return text
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.ToLowerInvariant())
                    .Distinct()
                    .Take(12)
                    .ToList();
        }

        private static string? ExtractOutputText(JsonElement root)
        {
            if (root.TryGetProperty("output_text", out var outputTextNode))
            {
               
                    return outputTextNode.GetString();
            }

            if (!root.TryGetProperty("output", out var outputNode) || outputNode.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            var parts = new List<string>();
            foreach (var outputItem in outputNode.EnumerateArray())
            {
                if (!outputItem.TryGetProperty("content", out var contentNode) || contentNode.ValueKind != JsonValueKind.Array)
                {
                        continue;
                }

                foreach (var contentItem in contentNode.EnumerateArray())
                {
                    if (!contentItem.TryGetProperty("text", out var textNode))
                    {
                        continue;
                    }

                        var value = textNode.GetString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        
                        parts.Add(value);
                    }
                }

            }

           
            return string.Join(",", parts);
        }
    }
}
