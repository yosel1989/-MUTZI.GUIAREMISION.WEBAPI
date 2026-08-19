using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo;

[Table("guia_remision_indicador_traslado")]
public partial class GuiaRemisionIndicadorTraslado
{
    [Key]
    [Column("guia_remision_indicador_traslado_id")]
    public int GuiaRemisionIndicadorTrasladoId { get; set; }

    [Column("name")]
    public string Descripcion { get; set; }

    [Column("code")]
    public string CodigoSunat { get; set; }

    [Column("active")]
    public bool Activo { get; set; } = true;

}
