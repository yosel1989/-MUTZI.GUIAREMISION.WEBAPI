using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    [SwaggerSchema("Recibe un objeto Unidad de Transporte para actualizar", Nullable = true)]
    public class UnidadTransporteUpdateDTO
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

        [JsonPropertyName("tarjeta")]
        [SwaggerSchema("Recibe un texto, se coloca el número de tarjeta de identificación vehicular.", Nullable = true)]
        [MaxLength(20)]
        public string? Tarjeta { get; set; }

        [JsonPropertyName("cod_emisor_vehicular")]
        [SwaggerSchema("Recibe un texto, se coloca el código del emisor vehicular, este campo es opcional", Nullable = true)]
        [StringLength(2)]
        public string? CodEmisorVehicular { get; set; }

        [JsonPropertyName("emisor_vehicular")]
        [SwaggerSchema("Recibe un texto, se coloca la abreviación del emisor vehicular. Ejm: 'MTC', este campo es opcional", Nullable = true)]
        [MinLength(2)]
        [MaxLength(100)]
        public string? EmisorVehicular { get; set; }

        [JsonPropertyName("nro_autorizacion")]
        [SwaggerSchema("Recibe un texto, se coloca el número de autorización del vehículo, este campo es opcional", Nullable = true)]
        [MinLength(3)]
        [MaxLength(50)]
        public string? NroAutorizacion { get; set; }

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
