using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryControl.DTO;
using InventoryControl.Services.Interfaces;
using StackExchange.Redis;
using System.Security.Claims;

namespace InventoryControl.Controllers.Api;

[ApiController]
[Route("core/auth")]
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

        return Ok(result);
    }

}
    

