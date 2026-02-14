using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter04.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorDemoController : ControllerBase
{
    private readonly ILogger<ErrorDemoController> _logger;

    public ErrorDemoController(ILogger<ErrorDemoController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Демонстрация: Выбросить исключение
    /// </summary>
    [HttpGet("throw")]
    public IActionResult ThrowException()
    {
        throw new InvalidOperationException("Это тестовое исключение для демонстрации обработки ошибок");
    }

    /// <summary>
    /// Демонстрация: Возврат BadRequest
    /// </summary>
    [HttpGet("badrequest")]
    public IActionResult BadRequestDemo()
    {
        return BadRequest(new { error = "Это пример BadRequest ответа" });
    }

    /// <summary>
    /// Демонстрация: Валидация с ошибкой
    /// </summary>
    [HttpPost("validate")]
    public IActionResult ValidateDemo([FromBody] ValidationRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            ModelState.AddModelError("Name", "Имя обязательно");
        }

        if (request.Age < 0 || request.Age > 150)
        {
            ModelState.AddModelError("Age", "Возраст должен быть от 0 до 150");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(new { message = "Валидация прошла успешно", data = request });
    }

    /// <summary>
    /// Демонстрация: Успешный ответ
    /// </summary>
    [HttpGet("success")]
    public IActionResult Success()
    {
        return Ok(new { message = "Запрос выполнен успешно" });
    }
}

public class ValidationRequest
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
