using InventoryControl.DTO;
using InventoryControl.Entity;

namespace InventoryControl.Service.Interfaces;

public interface IDOService
{
    Task<List<DOResponseDto>> GetAllAsync();
    Task<DO?> GetByIdAsync(string id);
    Task CreateAsync(DOCreateRequest request, string createdBy);
    Task DeleteAsync(string id);
    Task UpdateStatusAsync(string id, string status);
}