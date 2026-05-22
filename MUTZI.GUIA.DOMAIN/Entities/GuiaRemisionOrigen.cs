using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_origen")]
public partial class GuiaRemisionOrigen
{
    [Key]
    [Column("guia_remision_origen_id")]
    public int GuiaRemisionOrigenId { get; set; }

    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("direccion")]
    [MaxLength(250)]
    public string Direccion { get; set; } = null!;

    [Column("ubigeo_id")]
    [MaxLength(6)]
    public string UbigeoId { get; set; } = null!;

    [Column("pais")]
    [MaxLength(3)]
    public string Pais { get; set; } = null!;

    // Relaciones

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
