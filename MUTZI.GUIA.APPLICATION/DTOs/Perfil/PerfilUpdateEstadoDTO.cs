using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Perfil
{
    [SwaggerSchema("Recibe un objeto perfil para actualizar", Nullable = false)]
    public class PerfilUpdateEstadoDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Recibe un número, es el id del perfil", Nullable = false)]
        [Required(ErrorMessage = "El id del perfil es requerido")]
        public int PerfilId { get; set; }

        [JsonPropertyName("id_estado")]
        [SwaggerSchema("Recibe un número, aquí se coloca el id del estado", Nullable = false)]
        [Required]
        public int EstadoId { get; set; }
    }
}
