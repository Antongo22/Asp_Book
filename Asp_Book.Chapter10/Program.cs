using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Asp_Book.Chapter10.Hubs;
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

// SignalR
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Глава 10: SignalR",
        Version = "v1",
        Description = "Работа с SignalR - отправка сообщений одному и всем"
    });
});

var app = builder.Build();

// Swagger доступен всегда
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 10 API v1");
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
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// SignalR Hub
app.MapHub<ChatHub>("/chathub");

app.Run();
