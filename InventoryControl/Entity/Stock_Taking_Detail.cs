using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_Stock_Taking_Detail")]
public class StockTakingDetail
{
    [Key]
    [Column("st_detail_id")]
    public string StockTakingDetailId { get; set; }

    [Column("st_id")]
    public string StockTakingId { get; set; }

    [Column("tag_id")]
    public string TagId { get; set; }

    [Column("action")]
    public string? Action { get; set; }

    public StockTaking? StockTaking { get; set; }

    public Tag? Tag { get; set; }
}