using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchToSelectDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("entity_id")]
        public int EntityId { get; set; }

        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }

        [JsonPropertyName("entity_document_number")]
        public string EntityDocumentNumber { get; set; }

    }
}
