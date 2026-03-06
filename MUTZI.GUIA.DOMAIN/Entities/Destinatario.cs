using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class Destinatario
{
    public int DestinatarioId { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string UbigeoId { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string? CodigoSunat { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string? Estado { get; set; }

    public sbyte? IdEstado { get; set; }

    public virtual ICollection<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; } = new List<EmailDestinatarioGuid>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    public virtual Ubigeo Ubigeo { get; set; } = null!;
}
