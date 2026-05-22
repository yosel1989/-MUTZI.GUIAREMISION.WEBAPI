using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidad_medida")]
public partial class UnidadMedida
{
    [Key]
    [Column("unidad_medida_id")]
    public int UnidadMedidaId { get; set; }

    [Column("codigo_um")]
    [MaxLength(10)]
    public string CodigoUm { get; set; } = null!;

    [Column("descripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    [Column("es_unidad_peso")]
    public bool EsUnidadPeso { get; set; } = false;

    [Column("es_unidad_volumen")]
    public bool EsUnidadVolumen { get; set; } = false;

    [Column("es_unidad_longitud")]
    public bool EsUnidadLongitud { get; set; } = false;

    [Column("es_unidad_conteo")]
    public bool EsUnidadConteo { get; set; } = false;

    [Column("activo")]
    public bool Activo { get; set; }

    public virtual ICollection<GuiaRemisionDatoEnvio> DatosEnvios { get; set; } = new List<GuiaRemisionDatoEnvio>();

    public virtual ICollection<GuiaRemisionDetalle> GuiaDetalles { get; set; } = new List<GuiaRemisionDetalle>();
}
