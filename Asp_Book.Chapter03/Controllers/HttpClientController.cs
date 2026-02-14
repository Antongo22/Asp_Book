using System.Text.Json;
using Asp_Book.Chapter03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter03.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HttpClientController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IExternalApiService _externalApiService;
    private readonly ILogger<HttpClientController> _logger;

    public HttpClientController(
        IHttpClientFactory httpClientFactory,
        IExternalApiService externalApiService,
        ILogger<HttpClientController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _externalApiService = externalApiService;
        _logger = logger;
    }

    /// <summary>
    /// Демонстрация использования HttpClient напрямую
    /// </summary>
    [HttpGet("direct/{id}")]
    public async Task<IActionResult> GetPostDirect(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        try
        {
            var response = await client.GetAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<object>(content));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ошибка при запросе");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Получить пост через сервис
    /// </summary>
    [HttpGet("post/{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        try
        {
            var result = await _externalApiService.GetPostAsync(id);
            return Ok(JsonSerializer.Deserialize<object>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении поста");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Получить все посты
    /// </summary>
    [HttpGet("posts")]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var result = await _externalApiService.GetAllPostsAsync();
            return Ok(JsonSerializer.Deserialize<object>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении постов");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Создать новый пост
    /// </summary>
    [HttpPost("post")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        try
        {
            var result = await _externalApiService.CreatePostAsync(request.Title, request.Body, request.UserId);
            return Ok(JsonSerializer.Deserialize<object>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании поста");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
}
