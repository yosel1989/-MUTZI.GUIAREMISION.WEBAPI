using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("conductor")]
public partial class Conductor
{
    [Key]
    [Column("conductor_id")]
    public int ConductorId { get; set; }

    [Column("document_type_id")]
    public int TipoDocumentoId { get; set; }

    [Column("document_number")]
    [MaxLength(15)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("nombres")]
    [MaxLength(50)]
    public string Nombres { get; set; } = null!;

    [Column("apellidos")]
    [MaxLength(50)]
    public string Apellidos { get; set; } = null!;

    [Column("cargo")]
    [MaxLength(100)]
    public string? Cargo { get; set; }

    [Column("tipo")]
    [MaxLength(20)]
    public string? Tipo { get; set; }

    [Column("licencia")]
    [MaxLength(20)]
    public string Licencia { get; set; } = null!;

    [Column("created_at")]
    public DateTime FechaRegistro { get; set; }

    [Column("updated_at")]
    public DateTime? FechaModifico { get; set; }

    [Column("created_at_user")]
    [MaxLength(75)]
    public string UsuarioRegistro { get; set; }

    [Column("created_at_user_name")]
    [MaxLength(150)]
    public string UsuarioRegistroNombre { get; set; }

    [Column("updated_at_user")]
    [MaxLength(75)]
    public string? UsuarioModifico { get; set; }

    [Column("updated_at_user_name")]
    [MaxLength(150)]
    public string? UsuarioModificoNombre { get; set; }

    [Column("active")]
    public bool Active{ get; set; } = true;

    // Relaciones

    [ForeignKey(nameof(TipoDocumentoId))]
    public virtual TipoDocumento TipoDocumento { get; set; }

    public virtual ICollection<GuiaRemisionConductor> ConductorGuids { get; set; } = new List<GuiaRemisionConductor>();
}
