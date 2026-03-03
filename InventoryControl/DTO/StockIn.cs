namespace InventoryControl.DTO;


public class StockInRequestDto
{
    public string EpcTag { get; set; } = null!;
    public string ReaderId { get; set; } = null!;
}
public class StockInDto
{
    public List<string> SelectedTagIds { get; set; } = new();
    public List<string> ScannedTagIds { get; set; } = new();
    public string ReaderId { get; set; } = null!;
}