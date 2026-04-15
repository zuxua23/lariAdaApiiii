namespace InventoryControl.DTO;

public class TransactionHistoryDto
{
    public DateTime? TxDate { get; set; }
    public string TxType { get; set; }
    public string DoNumber { get; set; }
    public string ReaderName { get; set; }
    public string TagId { get; set; }
    public string ItemName { get; set; }
    public string LocationName { get; set; }
}