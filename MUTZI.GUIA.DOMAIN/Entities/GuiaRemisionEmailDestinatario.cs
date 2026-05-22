using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_email_destinatario")]
public partial class GuiaRemisionEmailDestinatario
{
    [Key]
    [Column("guia_remision_email_destinatario_id")]
    public int GuiaRemisionEmailDestinatarioId { get; set; }

    [Column("email")]
    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("establecimiento_id")]
    public int EstablecimientoId { get; set; }

    // Relaciones

    [ForeignKey(nameof(EstablecimientoId))]
    public virtual Establecimiento EstablecimientoDestinatario { get; set; } = null!;

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision Guia { get; set; } = null!;
}
