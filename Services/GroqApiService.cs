using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AIChatApp.Models;

namespace AIChatApp.Services;

public class GroqApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string ApiBaseUrl = "https://api.groq.com/openai/v1/chat/completions";

    public GroqApiService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AIChatApp/1.0");
    }

    public async Task<string> SendChatAsync(List<GroqMessage> messages, string model = "mixtral-8x7b-32768", CancellationToken cancellationToken = default)
    {
        var request = new GroqChatRequest
        {
            Model = model,
            Messages = messages,
            Temperature = 0.7,
            MaxTokens = 2048
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(ApiBaseUrl, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var groqResponse = JsonSerializer.Deserialize<GroqChatResponse>(responseJson);

            return groqResponse?.Choices?.FirstOrDefault()?.Message?.Content ?? "無回應";
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"API 請求失敗: {ex.Message}", ex);
        }
        catch (TaskCanceledException)
        {
            throw new Exception("請求已取消");
        }
        catch (Exception ex)
        {
            throw new Exception($"發生錯誤: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

