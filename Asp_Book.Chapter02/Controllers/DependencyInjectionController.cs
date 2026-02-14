using Asp_Book.Chapter02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DependencyInjectionController : ControllerBase
{
    private readonly ILogger<DependencyInjectionController> _logger;
    private readonly IScopedService _scopedService1;
    private readonly IScopedService _scopedService2;
    private readonly ISingletonService _singletonService1;
    private readonly ISingletonService _singletonService2;
    private readonly ITransientService _transientService1;
    private readonly ITransientService _transientService2;

    public DependencyInjectionController(
        ILogger<DependencyInjectionController> logger,
        IScopedService scopedService1,
        IScopedService scopedService2,
        ISingletonService singletonService1,
        ISingletonService singletonService2,
        ITransientService transientService1,
        ITransientService transientService2)
    {
        _logger = logger;
        _scopedService1 = scopedService1;
        _scopedService2 = scopedService2;
        _singletonService1 = singletonService1;
        _singletonService2 = singletonService2;
        _transientService1 = transientService1;
        _transientService2 = transientService2;
    }

    /// <summary>
    /// Демонстрация жизненных циклов сервисов
    /// </summary>
    [HttpGet("lifetimes")]
    public IActionResult GetLifetimes()
    {
        _logger.LogInformation("Демонстрация жизненных циклов DI");

        return Ok(new
        {
            scoped = new
            {
                service1 = new { id = _scopedService1.GetId(), type = _scopedService1.GetServiceType() },
                service2 = new { id = _scopedService2.GetId(), type = _scopedService2.GetServiceType() },
                explanation = "Scoped: один экземпляр на HTTP запрос. Оба сервиса должны иметь одинаковый ID."
            },
            singleton = new
            {
                service1 = new { id = _singletonService1.GetId(), type = _singletonService1.GetServiceType() },
                service2 = new { id = _singletonService2.GetId(), type = _singletonService2.GetServiceType() },
                explanation = "Singleton: один экземпляр на все приложение. Оба сервиса должны иметь одинаковый ID."
            },
            transient = new
            {
                service1 = new { id = _transientService1.GetId(), type = _transientService1.GetServiceType() },
                service2 = new { id = _transientService2.GetId(), type = _transientService2.GetServiceType() },
                explanation = "Transient: новый экземпляр каждый раз. Оба сервиса должны иметь разные ID."
            }
        });
    }

    /// <summary>
    /// Демонстрация использования ILogger через DI
    /// </summary>
    [HttpGet("logger-demo")]
    public IActionResult LoggerDemo([FromQuery] string message = "Тестовое сообщение")
    {
        _logger.LogInformation("Получен запрос на логирование: {Message}", message);
        _logger.LogWarning("Это предупреждение через DI логгер");
        _logger.LogError("Это ошибка через DI логгер (для демонстрации)");

        return Ok(new
        {
            message = "Сообщения залогированы. Проверьте консоль или логи приложения.",
            loggedMessage = message
        });
    }
}
