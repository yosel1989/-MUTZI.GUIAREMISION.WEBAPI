using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("docRelacionado", Schema = "logistica")]
public partial class DocRelacionado
{
    [Key]
    [Column("docRelacionadoID")]
    public int DocRelacionadoId { get; set; }

    [Column("guiaID")]
    public int GuiaId { get; set; }

    [Column("tipoDocRef")]
    [MaxLength(50)]
    public string? TipoDocRef { get; set; }

    [Column("numeroDocRef")]
    [MaxLength(30)]
    public string? NumeroDocRef { get; set; }

    [Column("rucDocRef")]
    [MaxLength(12)]
    public string? RucDocRef { get; set; }

    [ForeignKey(nameof(GuiaId))]
    public virtual GuiaRemision Guia { get; set; } = null!;
}
