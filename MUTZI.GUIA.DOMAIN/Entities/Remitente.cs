using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("remitente")]
public partial class Remitente
{
    [Key]
    [Column("remitenteID")]
    public int RemitenteId { get; set; }

    [Column("ruc")]
    [MaxLength(11)]
    public string Ruc { get; set; } = null!;

    [Column("descripcion")]
    [MaxLength(200)]
    public string Descripcion { get; set; } = null!;

    [Column("ubigeoID")]
    [MaxLength(6)]
    public string UbigeoId { get; set; } = null!;

    [Column("direccion")]
    [MaxLength(250)]
    public string Direccion { get; set; } = null!;

    [Column("pais")]
    [MaxLength(3)]
    public string Pais { get; set; } = null!;

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("serie")]
    [MaxLength(4)]
    public string Serie { get; set; } = null!;

    [Column("codigoSunat")]
    [MaxLength(4)]
    public string? CodigoSunat { get; set; }

    [Column("fechaCreacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column("fechaEdicion", TypeName = "datetime")]
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
    public string? Estado { get; set; }

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; }

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    [ForeignKey(nameof(Ruc))]
    [InverseProperty(nameof(Entities.VentaConfig.Remitentes))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;

    [ForeignKey(nameof(UbigeoId))]
    [InverseProperty(nameof(Entities.Ubigeo.Remitentes))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
