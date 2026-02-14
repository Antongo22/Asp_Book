using Asp_Book.Chapter12.Data;
using Asp_Book.Chapter12.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// In-Memory Database для демонстрации
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseInMemoryDatabase("ChatDb"));

builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Глава 12: Тестирование",
        Version = "v1",
        Description = "Тестирование API и EF Core с использованием In-Memory базы данных"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 12 API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
