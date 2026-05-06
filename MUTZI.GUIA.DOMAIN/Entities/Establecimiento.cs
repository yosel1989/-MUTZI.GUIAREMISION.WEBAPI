using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("establecimiento")]
public partial class Establecimiento
{
    [Key]
    [Column("establecimiento_id")]
    public int EstablecimientoId { get; set; }

    [Column("ruc")]
    public string Ruc { get; set; } = null!;

    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Column("ubigeo_id")]
    public string UbigeoId { get; set; } = null!;

    [Column("direccion")]
    public string Direccion { get; set; } = null!;

    [Column("pais")]
    public string Pais { get; set; } = null!;

    [Column("email")]
    public string? Email { get; set; }

    [Column("serie")]
    public string Serie { get; set; } = null!;

    [Column("codigo_sunat")]
    public string CodigoSunat { get; set; }

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

    [Column("estado")]
    public string Estado { get; set; } = "activo"; 

    [Column("id_estado")]
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
