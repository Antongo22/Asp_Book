using System.IO;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

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

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger доступен всегда для удобства разработки
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 1 API v1");
    c.RoutePrefix = "swagger";
});

// Отключаем HTTPS редирект в Docker (когда нет HTTPS порта)
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

app.MapControllers();
app.MapRazorPages();

app.Run();
