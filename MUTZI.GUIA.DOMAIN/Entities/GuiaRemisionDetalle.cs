using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_detalle")]
public partial class GuiaRemisionDetalle
{
    [Key]
    [Column("guia_remision_detalle_id")]
    public int GuiaRemisionDetalleId { get; set; }

    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("cantidad")]
    [MaxLength(10)]
    public string Cantidad { get; set; } = null!;

    [Column("codigo_um")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("codigo")]
    [MaxLength(20)]
    public string? Codigo { get; set; }

    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("codigo_sunat")]
    [MaxLength(45)]
    public string? CodigoSunat { get; set; }

    [Column("gtin")]
    [MaxLength(45)]
    public string? Gtin { get; set; }

    [Column("codigo_subnacional")]
    [MaxLength(45)]
    public string? CodigoSubnacional { get; set; }

    [Column("bien_normalizado")]
    public bool BienNormalizado { get; set; } = false;

    [ForeignKey(nameof(CodigoUm))]
    public virtual UnidadMedida UnidadMedida { get; set; } = null!;

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision GuiaRemision { get; set; } = null!;
}
