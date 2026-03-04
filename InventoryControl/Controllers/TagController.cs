using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[Authorize]
[ApiController]
[Route("taghttt")]
public class TagApiController : ControllerBase
{
    private readonly ITagService _service;

    public TagApiController(ITagService service)
    {
        _service = service;
    }

    [Authorize(Policy = "TRANS_TAG_VIEW")]
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [Authorize(Policy = "TRANS_TAG_CREATE")]
    [HttpPost]
    public async Task<IActionResult> Create(TagCreateDto dto)
    {
        var user = User.Identity?.Name ?? "system";
        await _service.CreateAsync(dto, user);
        return Ok();
    }
}