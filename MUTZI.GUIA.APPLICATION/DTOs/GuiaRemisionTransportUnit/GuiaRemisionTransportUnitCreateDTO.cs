using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionTransportUnit
{
    [SwaggerSchema("Devuelve un objeto de relación de la guía de remisión con la unidad de transporte")]
    public class GuiaRemisionTransportUnitCreateDTO
    {
        [JsonPropertyName("guia_remision_id")]
        [SwaggerSchema("Devuelve un entero, el ID de la guía a la que pertenece, en caso sea de una nueva guía el valor será 0", Nullable = false)]
        public int GuiaRemisionId { get; set; }

        [JsonPropertyName("transport_unit_id")]
        [SwaggerSchema("Devuelve un entero, el ID de la unidad de transporte", Nullable = false)]
        public int TransportUnitId { get; set; }

        [JsonPropertyName("type")]
        [SwaggerSchema("Devuelve un texto, el tipo de transporte para la guía si es 'principal' o 'secundario'", Nullable = false)]
        public string Type { get; set; }
    }
}
