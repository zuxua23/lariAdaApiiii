using InventoryControl.DTO;

namespace InventoryControl.Service.Interfaces;

public interface IStockOutService
{
    Task StockOutAsync(StockOutDto dto, string user);
}