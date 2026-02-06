using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class DocRelacionado
{
    public int DocRelacionadoId { get; set; }

    public int GuiaId { get; set; }

    public string? TipoDocRef { get; set; }

    public string? NumeroDocRef { get; set; }

    public string? RucDocRef { get; set; }

    public string? Estado { get; set; }

    public virtual GuiaRemision Guia { get; set; } = null!;
}
