using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class EmailDestinatarioGuid
{
    public int EmailDestinatarioGuidId { get; set; }

    public string Email { get; set; } = null!;

    public int GuiaId { get; set; }

    public int DestinatarioId { get; set; }

    public virtual Destinatario Destinatario { get; set; } = null!;

    public virtual GuiaRemision Guia { get; set; } = null!;
}
