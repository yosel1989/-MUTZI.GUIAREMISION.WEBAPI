using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo;

[Table("vw_master_unit_of_measurement")]
public partial class UnidadMedida
{
    [Key]
    [Column("unidad_medida_id")]
    public int UnidadMedidaId { get; set; }

    [Column("descripcion")]
    public string Descripcion { get; set; }

    [Column("descripcion_corta")]
    public string DescripcionCorta { get; set; }

    [Column("codigo_sunat")]
    public string CodigoSunat { get; set; }

    [Column("es_unidad_peso")]
    public bool EsUnidadPeso { get; set; } = false;

    [Column("es_unidad_volumen")]
    public bool EsUnidadVolumen { get; set; } = false;

    [Column("es_unidad_longitud")]
    public bool EsUnidadLongitud { get; set; } = false;

    [Column("es_unidad_conteo")]
    public bool EsUnidadConteo { get; set; } = false;

}
