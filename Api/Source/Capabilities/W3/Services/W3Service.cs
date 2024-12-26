using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Api.W3;

public record W3ValidationMessage(string Type, string Message);

public record W3ValidationResponse
{
    public W3ValidationMessage[] Messages { get; init; } = Array.Empty<W3ValidationMessage>();
}

public interface IW3Service
{
    Task<W3ValidationResponse> ValidateHtmlAsync(string html);
}

public class W3Service : IW3Service
{
    private readonly HttpClient _httpClient;
    private const string VALIDATOR_URL = "https://validator.w3.org/nu/?out=json";

    public W3Service(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Accessability Analyzer");
    }

    public async Task<W3ValidationResponse> ValidateHtmlAsync(string html)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, VALIDATOR_URL)
            {
                Content = new StringContent(html, Encoding.UTF8, "text/html"),
                Headers = 
                {
                    { "Accept", "application/json" }
                }
            };
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<W3ValidationResponse>() 
                   ?? new W3ValidationResponse();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to validate HTML: HTTP request failed", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse validation response", ex);
        }
    }
}
