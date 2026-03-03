using InventoryControl.DTO;

namespace InventoryControl.Service.Interfaces;

public interface IPrintTagRegisService
{
    Task<string> PrintAsync(PrintTagDto dto, string user);
    Task RegisterAsync(TagRegistrationDto dto, string user);
    Task<List<PrintHistoryResponseDto>> GetAvailableTagsAsync();
}