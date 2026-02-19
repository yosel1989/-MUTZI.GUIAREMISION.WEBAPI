using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado
{
    [SwaggerSchema("Recibe un objeto de documentos relacionados a la guía", Nullable = false)]
    public class DocRelacionadoCreateDTO
    {
        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Recibe un texto, es el tipo de documento vinculado como referencia en la guía de remisión, los tipos definidos son: FACTURA y BOLETA", Nullable = false)]
        [MaxLength(20)]
        [Required]
        public string TipoDocRef { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión. Ejm: F025-00000003", Nullable = false)]
        [MaxLength(21)]
        [Required]
        [NumeroDocRefFormato]
        public string NumeroDocRef { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión.", Nullable = false)]
        [StringLength(11)]
        [MinLength(11)]
        [Required]
        public string RucDocRef { get; set; }

        public string DocumentoAdicinalTipo()
        {
            if (string.IsNullOrEmpty(this.TipoDocRef))
                throw new System.Exception("tipo_doc_ref no puede estar vacío al generar el tipo de documento adicional.");

            switch (this.TipoDocRef.ToUpper())
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
