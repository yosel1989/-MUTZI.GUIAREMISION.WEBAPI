using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_documento_relacionado")]
public partial class GuiaRemisionDocumentoRelacionado
{
    [Key]
    [Column("guia_remision_documento_relacionado_id")]
    public int GuiaRemisionDocumentoRelacionadoId { get; set; }

    [Column("guia_remision_id")]
    public int GuiaRemisionId { get; set; }

    [Column("tipo_documento_referencia_id")]
    public int? TipoDocumentoReferenciaId { get; set; }

    [Column("numero_documento_referencia")]
    [MaxLength(30)]
    public string NumeroDocumentoReferencia { get; set; }

    [Column("ruc_documento_referencia")]
    [MaxLength(12)]
    public string RucDocumentoReferencia { get; set; }

    // Relaciones

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision Guia { get; set; } = null!;

    // Relaciones

    [ForeignKey(nameof(TipoDocumentoReferenciaId))]
    public virtual DocumentoRelacionado? TipoDocumentoReferencia { get; set; }
}
