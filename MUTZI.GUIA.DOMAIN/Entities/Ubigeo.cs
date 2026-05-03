using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("ubigeo")]
public partial class Ubigeo
{
    [Key]
    [Column("ubigeoID")]
    [MaxLength(6)]
    public string UbigeoId { get; set; } = null!;

    [Column("departamento")]
    [MaxLength(100)]
    public string? Departamento { get; set; }

    [Column("provincia")]
    [MaxLength(100)]
    public string? Provincia { get; set; }

    [Column("distrito")]
    [MaxLength(100)]
    public string? Distrito { get; set; }

    [InverseProperty(nameof(Entities.Destinatario.Ubigeo))]
    public virtual ICollection<Destinatario> Destinatarios { get; set; } = new List<Destinatario>();

    [InverseProperty(nameof(Entities.Destino.Ubigeo))]
    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    [InverseProperty(nameof(Entities.Origen.Ubigeo))]
    public virtual ICollection<Origen> Origens { get; set; } = new List<Origen>();

    [InverseProperty(nameof(Entities.Proveedor.Ubigeo))]
    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    [InverseProperty(nameof(Entities.RemiteTransportistum.Ubigeo))]
    public virtual ICollection<RemiteTransportistum> RemiteTransportista { get; set; } = new List<RemiteTransportistum>();

    [InverseProperty(nameof(Entities.Remitente.Ubigeo))]
    public virtual ICollection<Remitente> Remitentes { get; set; } = new List<Remitente>();
}
