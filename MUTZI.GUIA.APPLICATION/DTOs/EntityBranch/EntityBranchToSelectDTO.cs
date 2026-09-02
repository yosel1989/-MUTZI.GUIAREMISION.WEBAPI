using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchToSelectDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del remitente en la bbdd", Nullable = false)]
        public int EstablecimientoId { get; set; }

        [JsonPropertyName("description")]
        [SwaggerSchema("Devuelve el nombre asociada al establecimiento.", Nullable = false)]
        public string Description { get; set; }

        [JsonPropertyName("area")]
        [SwaggerSchema("Devuelve un texto, el area al que pertenece la serie , ejm: VENTAS, LOGISTICA", Nullable = true)]
        public string? Area { get; set; }
    }
}
