using Asp_Book.Chapter04.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 4 API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    // В production используем глобальный обработчик ошибок
    app.UseExceptionHandler("/Error");
}

// Глобальный обработчик исключений (должен быть первым)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Кастомный middleware для логирования запросов
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
