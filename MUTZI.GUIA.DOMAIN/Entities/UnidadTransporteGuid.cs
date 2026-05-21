using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidadTransporteGuid")]
public partial class UnidadTransporteGuid
{
    [Key]
    [Column("transporteGuidID")]
    public int TransporteGuidId { get; set; }

    [Column("transporteID")]
    public int? TransporteId { get; set; }

    [Column("datosEnvioID")]
    public int? DatosEnvioId { get; set; }

    [ForeignKey(nameof(DatosEnvioId))]
    [InverseProperty(nameof(Entities.GuiaRemisionDatoEnvio.TransporteGuids))]
    public virtual GuiaRemisionDatoEnvio? DatosEnvio { get; set; }

    [ForeignKey(nameof(TransporteId))]
    [InverseProperty(nameof(Entities.UnidadTransporte.TransporteGuids))]
    public virtual UnidadTransporte? Transporte { get; set; }
}
