using System.Text;
using System.Text.Json;

namespace Asp_Book.Chapter03.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(IHttpClientFactory httpClientFactory, ILogger<ExternalApiService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> GetPostAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ExternalAPI");
        
        try
        {
            _logger.LogInformation("Запрос поста с ID: {Id}", id);
            var response = await client.GetAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Получен ответ от API");
            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ошибка при запросе к внешнему API");
            throw;
        }
    }

    public async Task<string> GetAllPostsAsync()
    {
        var client = _httpClientFactory.CreateClient("ExternalAPI");
        
        try
        {
            _logger.LogInformation("Запрос всех постов");
            var response = await client.GetAsync("posts");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Получено {Count} постов", content.Length);
            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ошибка при запросе к внешнему API");
            throw;
        }
    }

    public async Task<string> CreatePostAsync(string title, string body, int userId)
    {
        var client = _httpClientFactory.CreateClient("ExternalAPI");
        
        try
        {
            var post = new
            {
                title = title,
                body = body,
                userId = userId
            };

            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Создание нового поста");
            var response = await client.PostAsync("posts", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Пост успешно создан");
            return responseContent;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ошибка при создании поста");
            throw;
        }
    }
}
