using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter08.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProtectedController : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetProtectedData()
    {
        var username = User.Identity?.Name ?? "Unknown";
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

        return Ok(new
        {
            message = "Доступ к защищённым данным получен!",
            user = new
            {
                id = userId,
                username,
                email,
                role
            },
            serverTime = DateTime.UtcNow
        });
    }
}
