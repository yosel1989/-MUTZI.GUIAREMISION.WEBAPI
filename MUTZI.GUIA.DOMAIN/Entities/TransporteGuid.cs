using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class TransporteGuid
{
    public int TransporteGuidId { get; set; }

    public int? TransporteId { get; set; }

    public int? DatosEnvioId { get; set; }

    public string? Estado { get; set; }

    public virtual DatosEnvio? DatosEnvio { get; set; }

    public virtual Transporte? Transporte { get; set; }
}
