using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("entity_branch")]
public partial class EntityBranch
{
    [Key]
    [Column("entity_branch_id")]
    public int EntityBranchId { get; set; }

    [Column("entity_id")]
    public int EntityId { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("ubigeo_id")]
    public string UbigeoId { get; set; } = null!;

    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("email")]
    public string? Email { get; set; }

    [Column("serie")]
    public string Serie { get; set; } = null!;

    [Column("code_sunat")]
    public string CodeSunat { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_at_user")]
    public string CreatedAtUser { get; set; }

    [Column("created_at_user_name")]
    public string CreatedAtUserName { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_at_user")]
    public string? UpdatedAtUser { get; set; }

    [Column("updated_at_user_name")]
    public string? UpdatedAtUserName { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("type")]
    public string Type { get; set; }

    [Column("area")]
    public string? Area { get; set; }

    [Column("is_main")]
    public bool IsMain { get; set; } = false;

    // Relaciones

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;

    [ForeignKey(nameof(EntityId))]
    public virtual Entity? Entity { get; set; }


}
