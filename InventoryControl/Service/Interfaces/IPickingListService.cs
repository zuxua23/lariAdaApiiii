using InventoryControl.DTO;
using InventoryControl.Entity;

namespace InventoryControl.Service.Interfaces;

public interface IPickingListService
{
    Task<List<DOResponseDto>> GetAllAsync();
    Task CreateAsync(PickingListDTO dto, string createdBy);
    Task UpdateAsync(string id, PickingListUpdateDTO dto);
    Task DeleteAsync(string id);
    Task UpdateStatusAsync(string id, string status);
    Task<DOResponseDto?> GetByIdAsync(string id);
}