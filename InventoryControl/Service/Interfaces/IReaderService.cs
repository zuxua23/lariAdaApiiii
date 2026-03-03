using InventoryControl.DTO;

namespace InventoryControl.Service.Interfaces;

public interface IReaderService
{
    Task<List<ReaderResponseDto>> GetAllAsync();
    Task CreateAsync(ReaderDto dto, string createdBy);
    Task DeleteAsync(string id);
}