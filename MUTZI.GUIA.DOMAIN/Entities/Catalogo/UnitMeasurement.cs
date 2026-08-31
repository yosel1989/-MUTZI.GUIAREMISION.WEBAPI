using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo;

[Table("vw_master_unit_of_measurement")]
public partial class UnitMeasurement
{
    [Key]
    [Column("unit_measurement_id")]
    public int UnidadMedidaId { get; set; }

    [Column("name")]
    public string Descripcion { get; set; }

    [Column("short_name")]
    public string DescripcionCorta { get; set; }

    [Column("sunat_code")]
    public string CodigoSunat { get; set; }

    [Column("is_unit_weight")]
    public bool EsUnidadPeso { get; set; } = false;

    [Column("is_unit_volume")]
    public bool EsUnidadVolumen { get; set; } = false;

    [Column("is_unit_length")]
    public bool EsUnidadLongitud { get; set; } = false;

    [Column("is_unit_counting")]
    public bool EsUnidadConteo { get; set; } = false;

    [Column("is_waybill")]
    public bool EsGuiaRemision { get; set; } = false;

}
