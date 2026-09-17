using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("entity_branch_serie")]
public partial class EntityBranchSerie
{
    [Key]
    [Column("entity_branch_serie_id")]
    public int EntityBranchSerieId { get; set; }

    [Column("entity_branch_id")]
    public int EntityBranchId { get; set; }

    [Column("invoice_type_id")]
    public int InvoiceTypeId { get; set; }

    [Column("serie")]
    public string Serie { get; set; }

    [Column("area")]
    public string Area { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

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

    // Relaciones

    [ForeignKey(nameof(EntityBranchId))]
    public virtual EntityBranch EntityBranch { get; set; } = null!;

    [ForeignKey(nameof(InvoiceTypeId))]
    public virtual InvoiceType InvoiceType { get; set; }


}
