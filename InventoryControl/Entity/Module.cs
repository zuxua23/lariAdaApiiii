using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Entity;

[Table("tb_Module")]
public class Module
{
    [Key]
    [Column("id")]
    public string Id { get; set; }

    [Column("module_key")]
    public string ModuleKey { get; set; }

    [Column("module_name")]
    public string ModuleName { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; } = true;

    public ICollection<Permission> Permissions { get; set; }
}
