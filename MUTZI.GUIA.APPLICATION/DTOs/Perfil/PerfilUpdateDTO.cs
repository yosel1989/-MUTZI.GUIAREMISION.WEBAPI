using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Perfil
{
    [SwaggerSchema("Recibe un objeto perfil para actualizar", Nullable = false)]
    public class PerfilUpdateDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Recibe un número, es el id del perfil", Nullable = false)]
        [Required(ErrorMessage = "El id del perfil es requerido")]
        public int PerfilId { get; set; }

        [JsonPropertyName("nombre")]
        [SwaggerSchema("Recibe un texto, es el nombre del perfil", Nullable = false)]
        [Required(ErrorMessage = "El nombre del perfil es obligatoria")]
        [StringLength(75, ErrorMessage = "El nombre del perfil no puede exceder los 75 caracteres")]
        public string Nombre { get; set; }

        [JsonPropertyName("Descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripció del perfil", Nullable = true)]
        [MaxLength(150)]
        public string? Descripcion { get; set; }

        [JsonPropertyName("Codigo")]
        [SwaggerSchema("Recibe un número, aquí se coloca el código del perfil", Nullable = false)]
        [Required]
        public int Codigo { get; set; }
    }
}
