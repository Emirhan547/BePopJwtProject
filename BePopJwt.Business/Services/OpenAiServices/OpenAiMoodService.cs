using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BePopJwt.Business.Services.OpenAiServices
{
    public class OpenAiMoodService(HttpClient httpClient, IConfiguration configuration) : IOpenAiMoodService
    {
        public async Task<IReadOnlyList<string>> GetMoodKeywordsAsync(string mood, CancellationToken cancellationToken = default)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(mood))
            {
                return [];
            }

            var model = configuration["OpenAI:Model"] ?? "gpt-4.1-mini";
            var requestBody = new
            {
                model,
                input = $"Ruh hali: {mood}. Müzik önerisi için en fazla 8 İngilizce anahtar kelime üret. Sadece virgülle ayrılmış liste döndür."
            };

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

            string? text = null;
            if (document.RootElement.TryGetProperty("output_text", out var outputTextNode))
            {
                text = outputTextNode.GetString();
            }

            if (string.IsNullOrWhiteSpace(text) &&
                document.RootElement.TryGetProperty("output", out var outputNode) &&
                outputNode.ValueKind == JsonValueKind.Array)
            {
                var parts = new List<string>();
                foreach (var outputItem in outputNode.EnumerateArray())
                {
                    if (!outputItem.TryGetProperty("content", out var contentNode) || contentNode.ValueKind != JsonValueKind.Array)
                    {
                        continue;
                    }

                    foreach (var contentItem in contentNode.EnumerateArray())
                    {
                        if (contentItem.TryGetProperty("text", out var textNode))
                        {
                            var value = textNode.GetString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                parts.Add(value);
                            }
                        }
                    }
                }

                text = string.Join(",", parts);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return [];
            }

            return text
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLowerInvariant())
                .Distinct()
                .Take(8)
                .ToList();
        }
    }
}