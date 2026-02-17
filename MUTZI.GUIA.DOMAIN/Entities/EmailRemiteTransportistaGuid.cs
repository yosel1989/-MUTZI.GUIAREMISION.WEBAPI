using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class EmailRemiteTransportistaGuid
{
    public int EmailRemiteTransportistaGuidId { get; set; }

    public string Email { get; set; } = null!;

    public int GuiaId { get; set; }

    public int RemiteTransportistaId { get; set; }

    public virtual GuiaRemision Guia { get; set; } = null!;

    public virtual RemiteTransportistum RemiteTransportista { get; set; } = null!;
}
