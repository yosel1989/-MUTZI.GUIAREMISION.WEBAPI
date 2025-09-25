using System;
using System.Collections.Generic;

namespace GuiaRemision.Models.DB;

public partial class Ubigeo
{
    public string UbigeoId { get; set; } = null!;

    public string? Departamento { get; set; }

    public string? Provincia { get; set; }

    public string? Distrito { get; set; }

    public virtual ICollection<Destinatario> Destinatarios { get; set; } = new List<Destinatario>();

    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    public virtual ICollection<Origen> Origens { get; set; } = new List<Origen>();

    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    public virtual ICollection<RemiteTransportistum> RemiteTransportista { get; set; } = new List<RemiteTransportistum>();

    public virtual ICollection<Remitente> Remitentes { get; set; } = new List<Remitente>();
}
