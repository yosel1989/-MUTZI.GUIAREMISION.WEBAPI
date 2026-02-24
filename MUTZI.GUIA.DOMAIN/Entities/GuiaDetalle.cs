using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class GuiaDetalle
{
    public int GuiaDetalleId { get; set; }

    public int GuiaId { get; set; }

    public string? Codigo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Cantidad { get; set; } = null!;

    public string CodigoUm { get; set; } = null!;

    public virtual UnidadMedidum CodigoUmNavigation { get; set; } = null!;

    public virtual GuiaRemision Guia { get; set; } = null!;
}
