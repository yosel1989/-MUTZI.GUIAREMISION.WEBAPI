using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Recibe un objeto unidad_transporte.('En caso de guía de remisión remitente se completa solo si el tipo_transporte es 'PRIVADO', de lo contrario, no se completa nada.' && 'En caso de guía de remisión transportista si es obligatorio este objeto unidad_transporte')", Nullable = true)]
    public class UnidadTransporteCreateDTO
    {
        [JsonPropertyName("descripcion")]
        [SwaggerSchema("Recibe un texto, se coloca una descripción del vehículo, Ejm: CAMIONETA, CUSTER, etc.", Nullable = true)]
        [MaxLength(50)]
        public string? Descripcion { get; set; }

        [JsonPropertyName("marca")]
        [SwaggerSchema("Recibe un texto, se coloca la marca del vehículo, Ejm: TOYOTA, NISSAN, etc.", Nullable = true)]
        [MaxLength(20)]
        public string? Marca { get; set; }

        [JsonPropertyName("modelo")]
        [SwaggerSchema("Recibe un texto, se coloca el modelo del vehículo, Ejm: 2020, 2021, etc.", Nullable = true)]
        [MaxLength(20)]
        public string? Modelo { get; set; }

        [JsonPropertyName("placa")]
        [SwaggerSchema("Recibe un texto, se coloca la placa de la unidad_transporte", Nullable = false)]
        [MaxLength(8)]
        [Required]
        public string Placa { get; set; }

        [JsonPropertyName("numero_registro_mtc")]
        [SwaggerSchema("Recibe un texto, se coloca el número de registro MTC del vehículo", Nullable = true)]
        [MaxLength(20)]
        public string? NumeroRegistroMTC { get; set; }

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Recibe un texto, se coloca el número de tarjeta de identificación vehicular.", Nullable = true)]
        [MaxLength(20)]
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
