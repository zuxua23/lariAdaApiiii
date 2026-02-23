using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_Transaction_Detail")]
public class Transaction_Detail
{
    [Key]
    [Column("trs_detail_id")]
    public string TransactionDetailId { get; set; }

    [Required]
    [Column("trs_id")]
    public string TransactionId { get; set; }

    [Required]
    [Column("tag_id")]
    public string TagId { get; set; }

    [Required]
    [Column("itm_id")]
    public string ItemId { get; set; }

    public Transaction Transaction { get; set; }
    public Tag Tag { get; set; }
    public Item Item { get; set; }

}