using System;
using System.Collections.Generic;

namespace MUTZI.GUIA.DOMAIN.Entities;

public partial class GuiaRemision
{
    public int GuiaId { get; set; }

    public string NumeroGuia { get; set; } = null!;

    public string Ruc { get; set; } = null!;

    public string Serie { get; set; } = null!;

    public string Correlativo { get; set; } = null!;

    public string TipoGuia { get; set; } = null!;

    public string TipoTransporte { get; set; } = null!;

    public string TipoTraslado { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public string Hora { get; set; } = null!;

    public string? Observacion { get; set; }

    public int RemitenteId { get; set; }

    public int? RemiteTransportistaId { get; set; }

    public int DestinatarioId { get; set; }

    public int? ProveedorId { get; set; }

    public string? Area { get; set; }

    public string? RegistroMtc { get; set; }

    public string RespuestaTicket { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string Estado { get; set; } = null!;

    public sbyte IdEstado { get; set; }

    public virtual ICollection<DatosEnvio> DatosEnvios { get; set; } = new List<DatosEnvio>();

    public virtual Destinatario Destinatario { get; set; } = null!;

    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    public virtual ICollection<DocRelacionado> DocRelacionados { get; set; } = new List<DocRelacionado>();

    public virtual ICollection<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; } = new List<EmailDestinatarioGuid>();

    public virtual ICollection<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; } = new List<EmailRemiteTransportistaGuid>();

    public virtual ICollection<GuiaDetalle> GuiaDetalles { get; set; } = new List<GuiaDetalle>();

    public virtual ICollection<Origen> Origens { get; set; } = new List<Origen>();

    public virtual Proveedor? Proveedor { get; set; }

    public virtual RemiteTransportistum? RemiteTransportista { get; set; }

    public virtual Remitente Remitente { get; set; } = null!;

    public virtual VentaConfig RucNavigation { get; set; } = null!;
}
