using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter07.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Требует авторизации
public class ProtectedController : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetProtectedData()
    {
        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "Это защищенные данные",
            username = username,
            role = role,
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")] // Требует роль Admin
    public IActionResult GetAdminData()
    {
        return Ok(new
        {
            message = "Это данные только для администраторов",
            timestamp = DateTime.UtcNow
        });
    }
}
