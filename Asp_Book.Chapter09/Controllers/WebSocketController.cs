using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter09.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebSocketController : ControllerBase
{
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            message = "WebSocket endpoint доступен по адресу ws://localhost:5009/ws",
            description = "Подключитесь к WebSocket для получения сообщений в реальном времени"
        });
    }
}
