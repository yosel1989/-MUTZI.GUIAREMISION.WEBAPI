using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class Conductor
{
    public int ConductorId { get; set; }

    public string? TipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public string Licencia { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string? Activo { get; set; }

    public virtual ICollection<DatosEnvio> DatosEnvios { get; set; } = new List<DatosEnvio>();
}
