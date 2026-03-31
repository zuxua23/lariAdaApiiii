using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Implementations;
using InventoryControl.Service.Interfaces;
using InventoryControl.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;


[ApiController]
[Route("api/reader")]
public class ReaderApiController : ControllerBase
{
    private readonly IReaderService _service;

    public ReaderApiController(IReaderService service)
    {
        _service = service;
    }

    [HttpGet]
    [AuthorizePermission]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [AuthorizePermission]
    public async Task<IActionResult> GetById(string id)
    {
        var reader = await _service.GetByIdAsync(id);
        if (reader == null)
            return NotFound(new { message = "Reader tidak ditemukan" });
        return Ok(reader);
    }

    [HttpPost]
    [AuthorizePermission]
    public async Task<IActionResult> Create(ReaderDto dto)
    {
        var user = User.Identity?.Name ?? "system";
        await _service.CreateAsync(dto, user);
        return Ok(new { message = "Reader berhasil dibuat" });
    }

    [HttpDelete("{id}")]
    [AuthorizePermission]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "User berhasil dihapus" });
    }

    [HttpPut("{id}")]
    [AuthorizePermission]
    public async Task<IActionResult> Update(string id, ReaderDto dto)
    {
        var user = User.Identity?.Name ?? "system";

        await _service.UpdateAsync(id, dto, user);

        return Ok(new { message = "Reader berhasil diupdate" });
    }
}