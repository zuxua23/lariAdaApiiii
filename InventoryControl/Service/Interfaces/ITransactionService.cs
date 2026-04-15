using InventoryControl.DTO;

namespace InventoryControl.Service.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionHistoryDto>> GetHistory(DateTime? fromDate, DateTime? toDate, string? txType);
}
