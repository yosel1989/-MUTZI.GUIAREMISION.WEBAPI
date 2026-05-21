using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidadMedida")]
public partial class UnidadMedida
{
    [Key]
    [Column("codigoUM")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("es_unidad_peso")]
    public int? EsUnidadPeso { get; set; }

    [Column("es_unidad_volumen")]
    public int? EsUnidadVolumen { get; set; }

    [Column("es_unidad_longitud")]
    public int? EsUnidadLongitud { get; set; }

    [Column("es_unidad_conteo")]
    public int? EsUnidadConteo { get; set; }

    [Column("estado")]
    [MaxLength(20)]
    public string? Estado { get; set; }

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; }

    public virtual ICollection<GuiaRemisionDatoEnvio> DatosEnvios { get; set; } = new List<GuiaRemisionDatoEnvio>();

    public virtual ICollection<GuiaRemisionDetalle> GuiaDetalles { get; set; } = new List<GuiaRemisionDetalle>();
}
