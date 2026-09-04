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
    public int? EntityId { get; set; }

    [Column("ruc")]
    public string? Ruc { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("ubigeo_id")]
    public string UbigeoId { get; set; } = null!;

    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("pais")]
    public string Pais { get; set; } = null!;

    [Column("email")]
    public string? Email { get; set; }

    [Column("serie")]
    public string Serie { get; set; } = null!;

    [Column("code_sunat")]
    public string CodeSunat { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }

    [Column("usuario_registro")]
    public string UsuarioRegistro { get; set; }

    [Column("usuario_registro_nombre")]
    public string UsuarioRegistroNombre { get; set; }

    [Column("usuario_modifico")]
    public string? UsuarioModifico { get; set; }

    [Column("usuario_modifico_nombre")]
    public string? UsuarioModificoNombre { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("tipo")]
    public string Tipo { get; set; }

    [Column("area")]
    public string? Area { get; set; }

    [Column("is_main")]
    public bool IsMain { get; set; } = false;

    // Relaciones

    [ForeignKey(nameof(Ruc))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;

    [ForeignKey(nameof(EntityId))]
    public virtual Entity? Entity { get; set; }


}
