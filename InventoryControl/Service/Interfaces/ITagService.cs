using InventoryControl.DTO;

namespace InventoryControl.Service.Interfaces;

public interface ITagService
{
    Task<List<TagResponseDto>> GetAllAsync();
    Task CreateAsync(TagCreateDto dto, string createdBy);
    Task UpdateStatusAsync(string tagId, string status);
}