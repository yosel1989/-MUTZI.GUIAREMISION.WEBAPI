using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionTransportUnit
{
    [SwaggerSchema("Devuelve un objeto de relación de la guía de remisión con la unidad de transporte")]
    public class GuiaRemisionTransportUnitDTO
    {
        [JsonPropertyName("guia_remision_id")]
        [SwaggerSchema("Devuelve un entero, el ID de la guía de remisión", Nullable = false)]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("transport_unit_id")]
        [SwaggerSchema("Devuelve un entero, el ID de la unidad de transporte", Nullable = false)]
        public int TransportUnitId { get; set; }

        [JsonPropertyName("type")]
        [SwaggerSchema("Devuelve un texto, el tipo de transporte para la guía si es 'principal' o 'secundario'", Nullable = false)]
        public string Marca { get; set; }

        [JsonPropertyName("created_at")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_at_user")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string CreatedAtUser { get; set; }

        [JsonPropertyName("created_at_user_name")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string CreatedAtUserName { get; set; }

        [JsonPropertyName("updated_at")]
        [SwaggerSchema("Devuelve un texto de fecha en formato: yyyy-MM-ddTHH:mm:ss; por ejemplo: 2025-09-23T14:35:00", Nullable = false)]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updated_at_user")]
        [SwaggerSchema("Devuelve un texto, el usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string? UpdatedAtUser { get; set; }

        [JsonPropertyName("updated_at_user_name")]
        [SwaggerSchema("Devuelve un texto, el nombre del usuario que ha registrado la transacción en el sistema", Nullable = false)]
        public string? UpdatedAtUserName { get; set; }
    }
}
