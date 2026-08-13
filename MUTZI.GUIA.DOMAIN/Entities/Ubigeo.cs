using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("vw_ubigeo")]
public partial class Ubigeo
{
    [Key]
    [Column("ubigeo_id")]
    [MaxLength(6)]
    public string UbigeoId { get; set; } = null!;

    [Column("department")]
    [MaxLength(100)]
    public string? Departamento { get; set; }

    [Column("province")]
    [MaxLength(100)]
    public string? Provincia { get; set; }

    [Column("district")]
    [MaxLength(100)]
    public string? Distrito { get; set; }

    [InverseProperty(nameof(Entities.GuiaRemisionDestino.Ubigeo))]
    public virtual ICollection<GuiaRemisionDestino> Destinos { get; set; } = new List<GuiaRemisionDestino>();

    [InverseProperty(nameof(Entities.GuiaRemisionOrigen.Ubigeo))]
    public virtual ICollection<GuiaRemisionOrigen> Origens { get; set; } = new List<GuiaRemisionOrigen>();

    [InverseProperty(nameof(Entities.Proveedor.Ubigeo))]
    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    [InverseProperty(nameof(Entities.Transportista.Ubigeo))]
    public virtual ICollection<Transportista> RemiteTransportista { get; set; } = new List<Transportista>();

}
