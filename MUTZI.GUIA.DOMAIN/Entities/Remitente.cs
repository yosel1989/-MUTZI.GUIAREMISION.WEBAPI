using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class Remitente
{
    public int RemitenteId { get; set; }

    public string Ruc { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string UbigeoId { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string? Email { get; set; }

    public string Serie { get; set; } = null!;

    public string? CodigoSunat { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    public virtual VentaConfig RucNavigation { get; set; } = null!;

    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
