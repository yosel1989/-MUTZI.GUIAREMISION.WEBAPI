using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario
{
    public class DestinatarioListDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del destinatario en la bbdd", Nullable = false)]
        public int DestinatarioID { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve un texto, el tipo de documento solo pueden tener los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número documento del destinatario.", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        [SwaggerSchema("Devuelve en texto la razón social del destinatario.", Nullable = false)]
        public string RazonSocial { get; set; }

        [JsonPropertyName("codigo_sunat")]
        [SwaggerSchema("Devuelve en texto el Código de establecimiento o de la Sucursal", Nullable = true)]
        public string? CodigoSunat { get; set; }
    }
}
