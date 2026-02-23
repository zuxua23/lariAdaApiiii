using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_Reader")]
public class Reader
{
    [Key]
    [Column("id")]
    public string Id { get; set; }

    [Required]
    [Column("rdr_id")]
    public string ReaderId { get; set; }

    [Required]
    [Column("loc_id")]
    public string Location { get; set; }

    [Required]
    [Column("rdr_name")]
    public string Name { get; set; }

    [Required]
    [Column("created_by")]
    public string CreatedBy { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("isDelete")]
    public int? IsDelete { get; set; }

    public Location LocationNavigation { get; set; }
}