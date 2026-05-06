using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Conductor
{
    [SwaggerSchema("Recibe un objeto conductor. ('En caso de guía de remisión remitente se completa solo si el tipo_transporte es 'PRIVADO', de lo contrario, no se completa nada.' && 'En caso de guía de remisión transportista si es obligatorio este objeto conductor')", Nullable = true)]
    public class ConductorCreateDTO
    {
        [JsonPropertyName("tipo_documento")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el tipo de documento del conductor. Solo recibe los siguientes valores: DNI, CE y PASAPORTE", Nullable = false)]
        [MaxLength(21)]
        [Required]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("numero_documento")]
        [SwaggerSchema("Recibe un texto, se coloca el número de documento del conductor. En caso de DNI, solo 8 dígitos, en otros casos acepta hasta 15 digitos", Nullable = false)]
        [MaxLength(15)]
        [MinLength(8)]
        [Required]
        public string NumeroDocumento { get; set; }

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
        [SwaggerSchema("Recibe un texto, se coloca el cargo del conductor", Nullable = true)]
        [MaxLength(100)]
        public string? Cargo { get; set; }

        [JsonPropertyName("licencia")]
        [SwaggerSchema("Recibe un texto, se coloca el número de licencia del conductor", Nullable = false)]
        [MinLength(9)]
        [MaxLength(10)]
        [Required]
        [RegularExpression("^[A-Z0-9]{9,10}$", ErrorMessage = "El N° de licencia o brevete debe ser alfanumérico de 9 a 10 dígitos.")]
        public string Licencia { get; set; }

    }
}
