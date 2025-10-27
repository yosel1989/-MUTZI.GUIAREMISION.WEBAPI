using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Recibe un objeto Unidad de Transporte para actualizar", Nullable = true)]
    public class UnidadTransporteUpdateDTO
    {
        [JsonPropertyName("placa")]
        [SwaggerSchema("Recibe un texto, se coloca la placa de la unidad_transporte", Nullable = false)]
        [MaxLength(8)]
        [Required]
        public string Placa { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Recibe un texto, se coloca la tarjeta de la unidad_transporte", Nullable = false)]
        [MaxLength(8)]
        [Required]
        public string Tarjeta { get; set; }

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
