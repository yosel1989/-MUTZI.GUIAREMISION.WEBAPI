using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    [SwaggerSchema("Devuelve lista de código de países", Nullable = false)]
    public class PaisDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve el id del país.", Nullable = false)]
        public int Id { get; set; }

        [JsonPropertyName("pais")]
        [SwaggerSchema("Devuelve en texto el nombre del país.", Nullable = false)]
        public string Pais { get; set; }

        [JsonPropertyName("codigo")]
        [SwaggerSchema("Devuelve en texto el código del país.", Nullable = false)]
        public string Codigo { get; set; }
    }
}
