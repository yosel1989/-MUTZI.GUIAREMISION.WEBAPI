using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidadTransporteGuid")]
public partial class UnidadTransporteGuid
{
    public int TransporteGuidId { get; set; }

    public int? TransporteId { get; set; }

    public int? DatosEnvioId { get; set; }

    public virtual DatosEnvio? DatosEnvio { get; set; }

    public virtual UnidadTransporte? Transporte { get; set; }
}
