using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("emailRemiteTransportistaGuid")]
public partial class EmailRemiteTransportistaGuid
{
    [Key]
    [Column("emailRemiteTransportistaGuidID")]
    public int EmailRemiteTransportistaGuidId { get; set; }

    [Column("email")]
    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("remiteTransportistaID")]
    public int RemiteTransportistaId { get; set; }

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    [ForeignKey(nameof(RemiteTransportistaId))]
    public virtual RemiteTransportistum RemiteTransportista { get; set; } = null!;
}
