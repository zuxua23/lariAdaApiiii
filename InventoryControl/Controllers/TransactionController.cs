using InventoryControl.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service)
    {
        _service = service;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        DateTime? fromDate,
        DateTime? toDate,
        string? txType)
    {
        var data = await _service.GetHistory(fromDate, toDate, txType);
        return Ok(data);
    }
}

