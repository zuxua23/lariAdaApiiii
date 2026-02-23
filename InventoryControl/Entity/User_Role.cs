using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_User_Role")]
public class User_Role
{
    [Key]
    [Column("id")]
    public string Id { get; set; }

    [Required]
    [Column("uro_id")]
    public string UroId { get; set; }

    [Required]
    [Column("rol_id")]
    public string RolId { get; set; }

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }

    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }

    public Role Role { get; set; }


}
