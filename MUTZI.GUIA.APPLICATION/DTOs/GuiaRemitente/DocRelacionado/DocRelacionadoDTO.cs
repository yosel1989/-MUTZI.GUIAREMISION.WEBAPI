using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado
{
    [SwaggerSchema("Devuelve el objeto de documentos relacionados a la guía", Nullable = false)]
    public class DocRelacionadoDTO
    {
        [JsonPropertyName("doc_relacionado_id")]
        [SwaggerSchema("Devuelve en entero el ID del doc_relacionado asociado a la guía.", Nullable = false)]
        public int DocRelacionadoId { get; set; }

        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Devuelve un texto, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son: FACTURA y BOLETA", Nullable = false)]
        public string TipoDocumentoReferencia { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión. Ejm: F025-00000003", Nullable = false)]
        public string NumeroDocumentoReferencia { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Devuelve un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión.", Nullable = false)]
        public string RucDocumentoReferencia { get; set; }
        public string DocumentoAdicinalTipo()
        {
            if (string.IsNullOrEmpty(this.TipoDocumentoReferencia))
                throw new System.Exception("tipo_doc_ref no puede estar vacío al generar el tipo de documento adicional.");

            switch (this.TipoDocumentoReferencia.ToUpper())
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
            }
        }
    }
}
