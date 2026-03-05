using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryControl.Controllers;

[Authorize]
[ApiController]
[Route("do")]
public class DOApiController : ControllerBase
{
    private readonly IDOService _service;

    public DOApiController(IDOService service)
    {
        _service = service;
    }

    [Authorize(Policy = "MASTER_DO_VIEW")]
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [Authorize(Policy = "MASTER_DO_CREATE")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DOCreateRequest request)
    {
        if (request.Details == null || !request.Details.Any())
            return BadRequest("Detail DO tidak boleh kosong");

        var createdBy = User.FindFirst(ClaimTypes.Name)?.Value ?? "system";

        await _service.CreateAsync(request, createdBy);

        return Ok(new { message = "DO berhasil dibuat" });
    }

    [Authorize(Policy = "TRANS_DO_DELETE")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Policy = "TRANS_DO_UPDATE_STATUS")]
    [HttpPut("status/{id}")]
    public async Task<IActionResult> UpdateStatus(string id, DOStatusUpdateDto dto)
    {
        await _service.UpdateStatusAsync(id, dto.Status);
        return Ok(new { message = "Status DO berhasil diperbarui" });
    }
}