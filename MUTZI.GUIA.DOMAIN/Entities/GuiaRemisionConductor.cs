using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_conductor")]
public partial class GuiaRemisionConductor
{
    [Key]
    [Column("guia_remision_conductor_id")]
    public int GuiaRemisionConductorId { get; set; }

    [Column("conductor_id")]
    public int? ConductorId { get; set; }

    [Column("guia_remision_dato_envio_id")]
    public int? GuiaRemisionDatoEnvioId { get; set; }


    // Relaciones

    [ForeignKey(nameof(ConductorId))]
    public virtual Conductor? Conductor { get; set; }

    [ForeignKey(nameof(GuiaRemisionDatoEnvioId))]
    public virtual GuiaRemisionDatoEnvio? DatosEnvio { get; set; }
}
