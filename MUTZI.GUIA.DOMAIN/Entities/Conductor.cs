using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("conductor")]
public partial class Conductor
{
    [Key]
    [Column("conductorID")]
    public int ConductorId { get; set; }

    [Column("tipoDocumento")]
    [MaxLength(20)]
    public string? TipoDocumento { get; set; }

    [Column("numeroDocumento")]
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

    [Column("fechaCreacion")]
    public DateTime? FechaCreacion { get; set; }

    [Column("fechaEdicion")]
    public DateTime? FechaEdicion { get; set; }

    [Column("empleadoIDCreacion")]
    public int? EmpleadoIdcreacion { get; set; }

    [Column("empleadoNombreCreacion")]
    [MaxLength(100)]
    public string? EmpleadoNombreCreacion { get; set; }

    [Column("empleadoIDEdicion")]
    public int? EmpleadoIdedicion { get; set; }

    [Column("empleadoNombreEdicion")]
    [MaxLength(100)]
    public string? EmpleadoNombreEdicion { get; set; }

    [Column("estado")]
    public string? Estado { get; set; } = "activo";

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; } = 1;

    public virtual ICollection<ConductorGuid> ConductorGuids { get; set; } = new List<ConductorGuid>();
}
