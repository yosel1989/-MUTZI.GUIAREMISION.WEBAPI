using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Perfil
{
    public class PerfilCreateDTO
    {
        [JsonPropertyName("nombre")]
        [SwaggerSchema("Recibe un texto, es el nombre del perfil", Nullable = false)]
        [Required(ErrorMessage = "El nombre del perfil es obligatoria")]
        [MaxLength(75, ErrorMessage = "El nombre del perfil no puede exceder los 75 caracteres")]
        public string Nombre { get; set; }

        [JsonPropertyName("Descripcion")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la descripción del perfil", Nullable = true)]
        [StringLength(150, ErrorMessage = "El nombre del perfil no puede exceder los 150 caracteres")]
        public string? Descripcion { get; set; }


        [JsonPropertyName("Codigo")]
        [SwaggerSchema("Recibe un número, aquí se coloca el código del perfil", Nullable = false)]
        [Required]
        public int Codigo { get; set; }
    }
}
