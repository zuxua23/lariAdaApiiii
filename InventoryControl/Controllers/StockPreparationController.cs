using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[Authorize]
[ApiController]
[Route("stock/preparation")]
public class StockPreparationController : ControllerBase
{
    private readonly IStockPreparationService _service;

    public StockPreparationController(IStockPreparationService service)
    {
        _service = service;
    }

    [Authorize(Policy = "TRANS_STOCK_PREPARATION")]
    [HttpPost]
    public async Task<IActionResult> Prepare(StockPreparationRequestDto dto)
    {
        var user = User.Identity?.Name ?? "system";
        await _service.PrepareAsync(dto, user);
        return Ok(new { message = "Tag berhasil diprepare" });
    }
}