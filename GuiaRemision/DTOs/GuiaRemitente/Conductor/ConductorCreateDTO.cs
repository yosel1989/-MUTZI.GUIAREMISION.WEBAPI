using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.Conductor
{
    [SwaggerSchema("Recibe un objeto conductor. ('En caso de guía de remisión remitente se completa solo si el tipo_transporte es 'PRIVADO', de lo contrario, no se completa nada.' && 'En caso de guía de remisión transportista si es obligatorio este objeto conductor')", Nullable = true)]
    public class ConductorCreateDTO
    {
        [JsonPropertyName("dni")]
        [SwaggerSchema("Recibe un texto, se coloca el dni del conductor", Nullable = false)]
        [MaxLength(8)]
        [Required]
        public string DNI { get; set; }

        [JsonPropertyName("nombres")]
        [SwaggerSchema("Recibe un texto, se colocan los nombres del conductor", Nullable = false)]
        [MaxLength(50)]
        [Required]
        public string Nombres { get; set; }

        [JsonPropertyName("apellidos")]
        [SwaggerSchema("Recibe un texto, se colocan los apellidos del conductor", Nullable = false)]
        [MaxLength(50)]
        [Required]
        public string Apellidos { get; set; }

        [JsonPropertyName("cargo")]
        [SwaggerSchema("Recibe un texto, se coloca el cargo del conductor", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string Cargo { get; set; }

        [JsonPropertyName("licencia")]
        [SwaggerSchema("Recibe un texto, se coloca el número de licencia del conductor", Nullable = false)]
        [MaxLength(10)]
        [Required]
        public string Licencia { get; set; }
    }
}
