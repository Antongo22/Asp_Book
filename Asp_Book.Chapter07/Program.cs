using System.IO;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Asp_Book.Chapter07.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

// Регистрация сервисов
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Настройка JWT Authentication
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "MySuperSecretKeyThatIsAtLeast32CharactersLong!";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "AspBook";
var audience = builder.Configuration["Jwt:Audience"] ?? "AspBookUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Настройка Swagger с поддержкой JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Глава 7: JWT Авторизация",
        Version = "v1",
        Description = "JWT токены, аутентификация и авторизация"
    });

    // Добавление поддержки JWT в Swagger
    // SecuritySchemeType.Http — Swagger сам подставит "Bearer " перед токеном
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен (без приставки Bearer — она подставится автоматически)",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Swagger доступен всегда
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Глава 7 API v1");
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

app.UseAuthentication(); // ВАЖНО: Должно быть перед UseAuthorization
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
