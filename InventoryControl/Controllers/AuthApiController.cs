using Microsoft.AspNetCore.Mvc;
using InventoryControl.DTO;
using InventoryControl.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace InventoryControl.Controllers.Api;

[ApiController]
[Route("auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _authService.ValidateUserAsync(dto);

        HttpContext.Session.SetString("UserId", result.UserId.ToString());
        HttpContext.Session.SetString("Username", result.Username);

        HttpContext.Session.SetString("Roles", JsonSerializer.Serialize(result.Roles));
        HttpContext.Session.SetString("Permissions", JsonSerializer.Serialize(result.Permissions));
        Console.WriteLine("Login Permissions:");
        foreach (var p in result.Permissions)
        {
            Console.WriteLine(p);
        }
        return Ok(new
        {
            message = "Login success"
        });
    }
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logout success" });
    }

}
    

