using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class Transporte
{
    public int TransporteId { get; set; }

    public string? Descripcion { get; set; }

    public string? Modelo { get; set; }

    public string? Marca { get; set; }

    public string Placa { get; set; } = null!;

    public string? Tarjeta { get; set; }

    public string? NumeroRegistroMtc { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string? Activo { get; set; }

    public virtual ICollection<TransporteGuid> TransporteGuids { get; set; } = new List<TransporteGuid>();
}
