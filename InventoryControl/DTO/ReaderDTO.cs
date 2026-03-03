namespace InventoryControl.DTO;

public class ReaderDto
{
    public string RdrId { get; set; } = null!;
    public string LocId { get; set; } = null!;
    public string RdrName { get; set; } = null!;
}

public class ReaderResponseDto
{
    public string RdrId { get; set; } = null!;
    public string RdrName { get; set; } = null!;
    public string LocId { get; set; } = null!;
    public string? LocationName { get; set; }
}