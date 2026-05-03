using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("remiteTransportista")]
public partial class RemiteTransportistum
{
    [Key]
    [Column("remiteTransportistaID")]
    public int RemiteTransportistaId { get; set; }

    [Column("tipoDocumento")]
    [MaxLength(20)]
    public string TipoDocumento { get; set; } = null!;

    [Column("numeroDocumento")]
    [MaxLength(15)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("razonSocial")]
    [MaxLength(200)]
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

    // Campos adicionales que mencionaste
    public string? RegistroMTC { get; set; }

    public string? EmailContacto { get; set; }

    public virtual ICollection<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; } = new List<EmailRemiteTransportistaGuid>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    [ForeignKey(nameof(UbigeoId))]
    [InverseProperty(nameof(Entities.Ubigeo.RemiteTransportista))]
    public virtual Ubigeo? Ubigeo { get; set; }
}
