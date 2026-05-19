using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("emailDestinatarioGuid")]
public partial class EmailDestinatarioGuid
{
    [Key]
    [Column("emailDestinatarioGuidID")]
    public int EmailDestinatarioGuidId { get; set; }

    [Column("email")]
    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("destinatarioID")]
    public int DestinatarioId { get; set; }

    [ForeignKey(nameof(DestinatarioId))]
    public virtual Establecimiento EstablecimientoDestinatario { get; set; } = null!;

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;
}
