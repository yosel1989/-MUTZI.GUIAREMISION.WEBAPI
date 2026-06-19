using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado
{
    [SwaggerSchema("Devuelve el objeto de documentos relacionados a la guía", Nullable = false)]
    public class DocRelacionadoDTO
    {
        [JsonPropertyName("doc_relacionado_id")]
        [SwaggerSchema("Devuelve en entero el ID del doc_relacionado asociado a la guía.", Nullable = false)]
        public int GuiaRemisionDocumentoRelacionadoId { get; set; }

        [JsonPropertyName("tipo_doc_ref_id")]
        [SwaggerSchema("Devuelve un número, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son del catalogo N° 61 registrados en base de datos", Nullable = false)]
        public int? TipoDocumentoReferenciaId { get; set; }

        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Devuelve un texto, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son: FACTURA y BOLETA", Nullable = false)]
        public string? TipoDocumentoReferencia { get; set; }

        [JsonPropertyName("tipo_doc_ref_codigo")]
        [SwaggerSchema("Recibe un texto, es el código de sunat del catalogo N° 61", Nullable = false)]
        public string TipoDocumentoReferenciaCodigo { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión. Ejm: F025-00000003", Nullable = false)]
        public string NumeroDocumentoReferencia { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión.", Nullable = false)]
        public string RucDocumentoReferencia { get; set; }
        public string DocumentoAdicionalTipo()
        {
            if (this.TipoDocumentoReferenciaId is null)
                throw new System.Exception("tipo_doc_ref no puede estar vacío al generar el tipo de documento adicional.");

            return this.TipoDocumentoReferenciaCodigo;

            /*switch (this.TipoDocumentoReferencia.ToUpper())
            {
                case "FACTURA":
                    return "01"; // Factura
                case "BOLETA":
                    return "03"; // Boleta
                case "NOTA DE CREDITO":
                    return "07"; // Nota de crédito
                case "NOTA DE DEBITO":
                    return "08"; // Nota de débito
                default:
                    throw new System.Exception("Catálogo No. 01: (tipo_doc_ref)Tipo de documento vinculado como referencia no definido.");
            }*/
        }
    }
}
