using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[Authorize]
[ApiController]
[Route("reader")]
public class ReaderApiController : ControllerBase
{
    private readonly IReaderService _service;

    public ReaderApiController(IReaderService service)
    {
        _service = service;
    }

    [Authorize(Policy = "MASTER_READER_VIEW")]
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [Authorize(Policy = "MASTER_READER_CREATE")]
    [HttpPost]
    public async Task<IActionResult> Create(ReaderDto dto)
    {
        var user = User.Identity?.Name ?? "system";
        await _service.CreateAsync(dto, user);
        return Ok(new { message = "Reader berhasil dibuat" });
    }
}