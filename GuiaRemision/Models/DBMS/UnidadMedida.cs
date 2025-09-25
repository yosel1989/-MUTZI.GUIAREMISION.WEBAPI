using System;
using System.Collections.Generic;

namespace GuiaRemision.Models.DBMS;

public partial class UnidadMedida
{
    public string CodigoUm { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int? EsUnidadPeso { get; set; }

    public int? EsUnidadVolumen { get; set; }

    public int? EsUnidadLongitud { get; set; }

    public int? EsUnidadConteo { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DatosEnvio> DatosEnvios { get; set; } = new List<DatosEnvio>();

    public virtual ICollection<GuiaDetalle> GuiaDetalles { get; set; } = new List<GuiaDetalle>();
}
