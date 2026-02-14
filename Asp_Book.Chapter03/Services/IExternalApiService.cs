namespace Asp_Book.Chapter03.Services;

public interface IExternalApiService
{
    Task<string> GetPostAsync(int id);
    Task<string> GetAllPostsAsync();
    Task<string> CreatePostAsync(string title, string body, int userId);
}
