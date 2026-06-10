using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Transportista
{
    public class TransportistaSugeridoDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, es el número ID del transportista.", Nullable = false)]
        public int TransportistaId { get; set; }

        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Devuelve en texto el tipo de documento del transportista, solo pueden tener los siguientes valores: DNI, CE, RUC y PASAPORTE", Nullable = false)]
        public string? TipoDocumento { get; set; }

        [JsonPropertyName("tipo_documento_id")]
        [SwaggerSchema("Devuelve en número, el tipo de documento del transportista", Nullable = false)]
        public int? TipoDocumentoId { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Devuelve en texto el número documento del transportista.", Nullable = false)]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; }
    }
}
