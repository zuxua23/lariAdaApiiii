namespace InventoryControl.DTO;


public class StockInRequestDto
{
    public string EpcTag { get; set; } = null!;
    public string ReaderId { get; set; } = null!;
}

public class StockInDto
{
    public string ScannerType { get; set; } // RFID / QR
    public List<string> ScannedCodes { get; set; }
}