using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("proveedor")]
public partial class Proveedor
{
    [Key]
    [Column("proveedorID")]
    public int ProveedorId { get; set; }

    [Column("tipoDocumento")]
    [MaxLength(20)]
    public string TipoDocumento { get; set; } = null!;

    [Column("numeroDocumento")]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("razonSocial")]
    [MaxLength(250)]
    public string RazonSocial { get; set; } = null!;

    [Column("ubigeoID")]
    [MaxLength(6)]
    public string? UbigeoId { get; set; }

    [Column("direccion")]
    [MaxLength(250)]
    public string? Direccion { get; set; }

    [Column("pais")]
    [MaxLength(3)]
    public string? Pais { get; set; }

    [Column("email")]
    [MaxLength(50)]
    public string? Email { get; set; }

    [Column("codigoSunat")]
    [MaxLength(4)]
    public string? CodigoSunat { get; set; }

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

    [Column("estado", TypeName = "enum('activo','inactivo)")]
    public string? Estado { get; set; }

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; }

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    [ForeignKey(nameof(UbigeoId))]
    [InverseProperty(nameof(Ubigeo.Proveedors))]
    public virtual Ubigeo? Ubigeo { get; set; }

}
