using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[Authorize]
[ApiController]
[Route("stock/out")]
public class StockOutController : ControllerBase
{
    private readonly IStockOutService _service;

    public StockOutController(IStockOutService service)
    {
        _service = service;
    }

    [Authorize(Policy = "TRANS_STOCK_OUT")]
    [HttpPost]
    public async Task<IActionResult> Finalize(StockOutDto dto)
    {
        var user = User.Identity?.Name ?? "system";
        await _service.StockOutAsync(dto, user);

        return Ok(new { message = "Stock Out berhasil difinalisasi" });
    }
}
