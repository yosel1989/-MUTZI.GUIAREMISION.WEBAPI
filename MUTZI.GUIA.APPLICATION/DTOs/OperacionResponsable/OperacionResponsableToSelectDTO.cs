using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable
{
    public class OperacionResponsableToSelectDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un número, es el id de la operación", Nullable = false)]
        public int OperacionResponsableId { get; set; }

        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Devuelve un texto, es el nombre de la operación", Nullable = false)]
        public string Descripcion { get; set; }
    }
}
