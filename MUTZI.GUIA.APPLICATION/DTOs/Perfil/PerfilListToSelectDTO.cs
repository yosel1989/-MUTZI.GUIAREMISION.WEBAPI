using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Perfil
{
    public class PerfilListToSelectDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve un entero, el ID del perfil en la bbdd", Nullable = false)]
        public int PerfilId { get; set; }

        [JsonPropertyName("nombre")]
        [SwaggerSchema("Devuelve en texto el nombre del perfil.", Nullable = false)]
        public string Nombre { get; set; }
    }
}
