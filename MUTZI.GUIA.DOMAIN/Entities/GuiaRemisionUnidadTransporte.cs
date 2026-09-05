using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_unidad_transporte")]
public partial class GuiaRemisionUnidadTransporte
{
    [Key]
    [Column("guia_remision_unidad_transporte_id")]
    public int GuiaRemisionUnidadTransporteId { get; set; }

    [Column("unidad_transporte_id")]
    public int? UnidadTransporteId { get; set; }

    [Column("guia_remision_dato_envio_id")]
    public int? GuiaRemisionDatoEnvioId { get; set; }

    [ForeignKey(nameof(GuiaRemisionDatoEnvioId))]
    [InverseProperty(nameof(Entities.GuiaRemisionDatoEnvio.GuiaRemisionUnidadesTransporte))]
    public virtual GuiaRemisionDatoEnvio? DatosEnvio { get; set; }

    [ForeignKey(nameof(UnidadTransporteId))]
    [InverseProperty(nameof(Entities.TransportUnit.TransporteGuids))]
    public virtual TransportUnit? UnidadTransporte { get; set; }
}
