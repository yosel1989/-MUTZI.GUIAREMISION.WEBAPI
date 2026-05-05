using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("establecimiento")]
public partial class Establecimiento
{
    [Key]
    [Column("establecimientoID")]
    public int EstablecimientoId { get; set; }

    [Column("ruc")]
    public string Ruc { get; set; } = null!;

    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Column("ubigeoID")]
    public string UbigeoId { get; set; } = null!;

    [Column("direccion")]
    public string Direccion { get; set; } = null!;

    [Column("pais")]
    public string Pais { get; set; } = null!;

    [Column("email")]
    public string? Email { get; set; }

    [Column("serie")]
    public string Serie { get; set; } = null!;

    [Column("codigoSunat")]
    public string CodigoSunat { get; set; }

    [Column("fechaCreacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [Column("fechaEdicion")]
    public DateTime? FechaEdicion { get; set; }

    [Column("empleadoIDCreacion")]
    public int EmpleadoIdcreacion { get; set; }

    [Column("empleadoNombreCreacion")]
    public string EmpleadoNombreCreacion { get; set; }

    [Column("empleadoIDEdicion")]
    public int? EmpleadoIdedicion { get; set; }

    [Column("EmpleadoNombreEdicion")]
    public string? EmpleadoNombreEdicion { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = "activo"; 

    [Column("idEstado")]
    public sbyte IdEstado { get; set; } = 1;

    [Column("tipo")]
    public string Tipo { get; set; }

    [ForeignKey(nameof(Ruc))]
    public virtual VentaConfig RucNavigation { get; set; } = null!;

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;

    //public virtual ICollection<GuiaRemision> GuiaRemisionsRemitente { get; set; } = new List<GuiaRemision>();

    //public virtual ICollection<GuiaRemision> GuiaRemisionsDestinatario { get; set; } = new List<GuiaRemision>();

}
