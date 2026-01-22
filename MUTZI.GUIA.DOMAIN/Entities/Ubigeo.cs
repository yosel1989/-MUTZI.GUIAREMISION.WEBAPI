using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("ubigeo")]
public partial class Ubigeo
{
    [Column("ubigeoID")]
    public string UbigeoId { get; set; } = null!;

    [Column("departamento")]
    public string? Departamento { get; set; }

    [Column("provincia")]
    public string? Provincia { get; set; }

    [Column("distrito")]
    public string? Distrito { get; set; }

    public virtual ICollection<Destinatario> Destinatarios { get; set; } = new List<Destinatario>();

    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    public virtual ICollection<Origen> Origens { get; set; } = new List<Origen>();

    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    public virtual ICollection<RemiteTransportista> RemiteTransportista { get; set; } = new List<RemiteTransportista>();

    public virtual ICollection<Remitente> Remitentes { get; set; } = new List<Remitente>();
}
