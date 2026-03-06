using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class ConductorGuid
{
    public int ConductorGuidId { get; set; }

    public int? ConductorId { get; set; }

    public int? DatosEnvioId { get; set; }

    public virtual Conductor? Conductor { get; set; }

    public virtual DatosEnvio? DatosEnvio { get; set; }
}
