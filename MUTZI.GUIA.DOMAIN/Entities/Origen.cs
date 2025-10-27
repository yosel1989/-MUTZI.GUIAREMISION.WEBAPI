using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class Origen
{
    public int OrigenId { get; set; }

    public int GuiaId { get; set; }

    public string Direccion { get; set; } = null!;

    public string UbigeoId { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
