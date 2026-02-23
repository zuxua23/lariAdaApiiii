using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_Role_Permission")]
public class Role_Permission
{
    [Key]
    [Column("id")]
    public string Id { get; set; } 

    [Required]
    [Column("rpr_id")]
    public string Code { get; set; }
    [Required]
    [Column("per_id")]
    public string PerId { get; set; }
    [Required]
    [Column("rol_id")]
    public string RolId { get; set; }

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("is_override")]
    public int? Override { get; set; }

    public Role Role { get; set; }
    public Permission Permission { get; set; }


}