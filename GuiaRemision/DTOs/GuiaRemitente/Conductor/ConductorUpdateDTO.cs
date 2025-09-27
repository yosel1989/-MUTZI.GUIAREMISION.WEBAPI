using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.Conductor
{
    [SwaggerSchema("Recibe un objeto conductor para actualizar")]
    public class ConductorUpdateDTO
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

        [JsonPropertyName("empleado_id_edicion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha editado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreEdicion { get; set; }
    }
}
