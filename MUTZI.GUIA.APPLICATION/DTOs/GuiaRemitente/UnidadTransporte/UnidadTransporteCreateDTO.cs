using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte
{
    public enum EnumTipoUnidadTransporte
    {
        INTERNO,
        EXTERNO
    }


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
        [RegularExpression("^[A-Z0-9]{6,8}$", ErrorMessage = "La placa debe tener entre 6 y 8 caracteres alfanuméricos en mayúscula.")]
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
        [StringLength(100, MinimumLength = 2)]
        public string? EmisorVehicular { get; set; }

        [JsonPropertyName("nro_autorizacion")]
        [SwaggerSchema("Recibe un texto, se coloca el número de autorización del vehículo, este campo es opcional", Nullable = true)]
        [StringLength(50, MinimumLength = 3)]
        public string? NroAutorizacion { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = true)]
        public int? EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha realizado la transacción en el sistema", Nullable = true)]
        [MaxLength(100)]
        public string? EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("tipo")]
        [SwaggerSchema("Recibe un texto, se coloca el tipo de la unidad de transporte, Ejm: INTERNO, INTERNO", Nullable = false)]
        [MaxLength(20)]
        [EnumDataType(typeof(EnumTipoUnidadTransporte))]
        public string Tipo { get; set; }
    }
}
