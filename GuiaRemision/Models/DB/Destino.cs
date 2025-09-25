using System;
using System.Collections.Generic;

namespace GuiaRemision.Models.DB;

public partial class Destino
{
    public int DestinoId { get; set; }

    public int GuiaId { get; set; }

    public string Direccion { get; set; } = null!;

    public string UbigeoId { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
