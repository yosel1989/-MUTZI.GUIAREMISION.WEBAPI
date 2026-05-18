using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("conductor", Schema = "logistica")]
public partial class Conductor
{
    [Key]
    [Column("conductor_id")]
    public int ConductorId { get; set; }

    [Column("tipo_documento")]
    [MaxLength(20)]
    public string TipoDocumento { get; set; }

    [Column("numero_documento")]
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

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }

    [Column("usuario_registro")]
    [MaxLength(75)]
    public string UsuarioRegistro { get; set; }

    [Column("usuario_registro_nombre")]
    [MaxLength(150)]
    public string UsuarioRegistroNombre { get; set; }

    [Column("usuario_modifico")]
    [MaxLength(75)]
    public string? UsuarioModifico { get; set; }

    [Column("usuario_modifico_nombre")]
    [MaxLength(150)]
    public string? UsuarioModificoNombre { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = "activo";

    [Column("id_estado")]
    public sbyte IdEstado { get; set; } = 1;

    public virtual ICollection<ConductorGuid> ConductorGuids { get; set; } = new List<ConductorGuid>();
}
