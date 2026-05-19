using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_destino")]
public partial class GuiaRemisionDestino
{
    [Key]
    [Column("destinoID")]
    public int DestinoId { get; set; }

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("direccion")]
    [MaxLength(250)]
    public string Direccion { get; set; } = null!;

    [Column("ubigeoID")]
    [MaxLength(6)]
    public string UbigeoId { get; set; } = null!;

    [Column("pais")]
    [MaxLength(3)]
    public string Pais { get; set; } = null!;

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
