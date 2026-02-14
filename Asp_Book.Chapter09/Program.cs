using Asp_Book.Chapter09.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 9 API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// WebSocket middleware должен быть до UseAuthorization
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
