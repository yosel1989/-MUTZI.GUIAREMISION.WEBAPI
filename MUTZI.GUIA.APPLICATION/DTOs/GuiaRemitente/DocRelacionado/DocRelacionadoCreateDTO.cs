using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado
{
    [SwaggerSchema("Recibe un objeto de documentos relacionados a la guía", Nullable = false)]
    public class DocRelacionadoCreateDTO
    {
        [JsonPropertyName("tipo_doc_ref_id")]
        [SwaggerSchema("Recibe un texto, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son: FACTURA y BOLETA", Nullable = false)]
        [Required]
        public int TipoDocumentoReferenciaId { get; set; }

        [JsonPropertyName("tipo_doc_ref_codigo")]
        [SwaggerSchema("Recibe un texto, es el código de sunat del catalogo N° 61", Nullable = false)]
        [MaxLength(20)]
        [Required]
        public string TipoDocumentoReferenciaCodigo { get; set; }

        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Recibe un texto, es el nombre de referencia del documento", Nullable = false)]
        [MaxLength(20)]
        [Required]
        public string TipoDocumentoReferenciaNombre { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión. Ejm: F025-00000003", Nullable = false)]
        [MaxLength(21)]
        [Required]
        //[NumeroDocRefFormato]
        public string NumeroDocumentoReferencia { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión.", Nullable = false)]
        [StringLength(11)]
        [MinLength(11)]
        [Required]
        public string RucDocumentoReferencia { get; set; }

    }
}
