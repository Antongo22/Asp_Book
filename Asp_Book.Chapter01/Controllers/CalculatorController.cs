using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    /// <summary>
    /// Сложение двух чисел
    /// </summary>
    [HttpPost("add")]
    public IActionResult Add([FromBody] CalculationRequest request)
    {
        var result = request.A + request.B;
        return Ok(new { result = result, operation = "add" });
    }

    /// <summary>
    /// Вычитание двух чисел
    /// </summary>
    [HttpPost("subtract")]
    public IActionResult Subtract([FromBody] CalculationRequest request)
    {
        var result = request.A - request.B;
        return Ok(new { result = result, operation = "subtract" });
    }

    /// <summary>
    /// Умножение двух чисел
    /// </summary>
    [HttpPost("multiply")]
    public IActionResult Multiply([FromBody] CalculationRequest request)
    {
        var result = request.A * request.B;
        return Ok(new { result = result, operation = "multiply" });
    }

    /// <summary>
    /// Деление двух чисел
    /// </summary>
    [HttpPost("divide")]
    public IActionResult Divide([FromBody] CalculationRequest request)
    {
        if (request.B == 0)
        {
            return BadRequest(new { error = "Деление на ноль невозможно" });
        }
        var result = request.A / (double)request.B;
        return Ok(new { result = result, operation = "divide" });
    }
}

public class CalculationRequest
{
    public double A { get; set; }
    public double B { get; set; }
}
