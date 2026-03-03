namespace InventoryControl.DTO;

public class StockPreparationRequestDto
{
    public string DoId { get; set; } = null!;
    public string EpcTag { get; set; } = null!;
    public string ReaderId { get; set; } = null!;
}
public class StockPreparationDto
{
    public string DoId { get; set; } = null!;
    public List<string> TagIds { get; set; } = new();
    public string ReaderId { get; set; } = null!;
}