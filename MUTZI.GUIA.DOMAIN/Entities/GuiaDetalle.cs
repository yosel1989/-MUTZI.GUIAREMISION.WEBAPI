using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guiaDetalle")]
public partial class GuiaDetalle
{
    [Key]
    [Column("guiaDetalleID")]
    public int GuiaDetalleId { get; set; }

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("codigo")]
    [MaxLength(20)]
    public string? Codigo { get; set; }

    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("cantidad")]
    [MaxLength(10)]
    public string Cantidad { get; set; } = null!;

    [Column("codigoUM")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [ForeignKey(nameof(CodigoUm))]
    public virtual UnidadMedidum CodigoUmNavigation { get; set; } = null!;

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;
}
