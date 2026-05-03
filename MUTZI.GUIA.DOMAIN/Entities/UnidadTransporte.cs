using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidadTransporte")]
public class UnidadTransporte
{
    [Key]
    [Column("transporteID")]
    public int TransporteId { get; set; }

    [Column("descripcion")]
    [MaxLength(50)]
    public string? Descripcion { get; set; }

    [Column("modelo")]
    [MaxLength(20)]
    public string? Modelo { get; set; }

    [Column("marca")]
    [MaxLength(20)]
    public string? Marca { get; set; }

    [Column("placa")]
    [MaxLength(20)]
    public string Placa { get; set; } = null!;

    [Column("tarjeta")]
    [MaxLength(20)]
    public string? Tarjeta { get; set; }

    [Column("codEmisorVehicular")]
    [MaxLength(2)]
    public string? CodEmisorVehicular { get; set; }

    [Column("emisorVehicular")]
    [MaxLength(100)]
    public string? EmisorVehicular { get; set; }

    [Column("nroAutorizacion")]
    [MaxLength(50)]
    public string? NroAutorizacion { get; set; }

    [Column("tipo")]
    [MaxLength(20)]
    public string? Tipo { get; set; }

    [Column("fechaCreacion")]
    public DateTime? FechaCreacion { get; set; } = DateTime.UtcNow;

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

    [Column("estado", TypeName = "enum('activo','inactivo')")]
    public string? Estado { get; set; } = "activo";

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; } = 1;

    public virtual ICollection<UnidadTransporteGuid> TransporteGuids { get; set; } = new List<UnidadTransporteGuid>();
}
