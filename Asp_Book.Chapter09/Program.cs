using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Asp_Book.Chapter09.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Настройка Data Protection для Docker
try
{
    var keysDir = new DirectoryInfo("/app/keys");
    if (!keysDir.Exists)
    {
        keysDir.Create();
    }
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(keysDir)
        .SetApplicationName("Asp_Book");
}
catch
{
    // Если не удалось создать директорию, используем in-memory хранилище
    builder.Services.AddDataProtection()
        .SetApplicationName("Asp_Book");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Глава 9: WebSockets",
        Version = "v1",
        Description = "Работа с WebSockets - отправка сообщений одному и всем"
    });
});

var app = builder.Build();

// Swagger доступен всегда
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 9 API v1");
    c.RoutePrefix = "swagger";
});

// Отключаем HTTPS редирект в Docker
if (!app.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Contains("https") != true)
{
    // В Docker без HTTPS пропускаем редирект
}
else
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

// WebSocket middleware должен быть до UseAuthorization
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
