using InventoryControl.DTO;
//using InventoryControl.PermissionHelper;
using InventoryControl.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("user")]
public class UserApiController : ControllerBase
{
    private readonly IUserService _service;

    public UserApiController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null)
            return NotFound();

        return Ok(data);
    }


    [HttpPost]
    public async Task<IActionResult> Create(UserDto dto)
    {
        var createdBy = Request.Headers["X-User-Id"].FirstOrDefault() ?? "system";
        Console.WriteLine("User Header: " + Request.Headers["X-User-Id"]);
        await _service.CreateAsync(dto, createdBy);
        return Ok(new { message = "User berhasil dibuat" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        var updatedBy = Request.Headers["X-User-Id"].FirstOrDefault() ?? "system";
        Console.WriteLine("User Header: " + Request.Headers["X-User-Id"]);
        await _service.UpdateAsync(id, dto, updatedBy);
        return Ok(new { message = "User berhasil diperbarui" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "User berhasil dihapus" });
    }
}
