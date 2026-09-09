using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchListToModalDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("description")]
        [SwaggerSchema("Devuelve el nombre asociada al establecimiento.", Nullable = false)]
        public string Description { get; set; }

        [JsonPropertyName("code_sunat")]
        [SwaggerSchema("Devuelve el código de establecimiento registrado en SUNAT. Ejem: 0001, 0012", Nullable = false)]
        public string CodeSunat { get; set; }

        [JsonPropertyName("entity_id")]
        public int EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve un texto, el area al que pertenece la serie , ejm: VENTAS, LOGISTICA", Nullable = true)]
        public string? Area { get; set; }

        [JsonPropertyName("serie")]
        [SwaggerSchema("Devuelve un texto, la serie , ejm: T001, T002", Nullable = false)]
        public string Serie { get; set; }
    }
}
