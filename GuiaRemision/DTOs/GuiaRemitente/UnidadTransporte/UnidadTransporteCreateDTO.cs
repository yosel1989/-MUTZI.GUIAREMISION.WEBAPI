using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Recibe un objeto unidad_transporte.('En caso de guía de remisión remitente se completa solo si el tipo_transporte es 'PRIVADO', de lo contrario, no se completa nada.' && 'En caso de guía de remisión transportista si es obligatorio este objeto unidad_transporte')", Nullable = true)]
    public class UnidadTransporteCreateDTO
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

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = true)]
        public int? EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha realizado la transacción en el sistema", Nullable = true)]
        [MaxLength(100)]
        public string? EmpleadoNombreCreacion { get; set; }
    }
}
