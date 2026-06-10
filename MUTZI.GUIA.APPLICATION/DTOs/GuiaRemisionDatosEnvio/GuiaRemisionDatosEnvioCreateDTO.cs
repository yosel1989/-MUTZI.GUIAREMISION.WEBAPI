using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.DatosEnvio
{
    [SwaggerSchema("Recibe un objeto datos_envio, aquí se detala los detalles del envío", Nullable = false)]
    public class GuiaRemisionDatosEnvioCreateDTO
    {
        [JsonPropertyName("motivo_envio")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el motivo de envío. Ejm: Venta de mercadería, etc.", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string MotivoEnvio { get; set; }

        [JsonPropertyName("fecha_envio")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false)]
        [StringLength(10)]
        [MinLength(10)]
        [Required]
        public string FechaEnvio { get; set; }

        [JsonPropertyName("peso_bruto")]
        [SwaggerSchema("Recibe un decimal, aquí debe colocarse el peso bruto de todo el envío, Ejm: 20, 50, etc.", Nullable = false)]
        [Required]
        public decimal PesoBruto { get; set; }

        [JsonPropertyName("unidad_medida_id")]
        [SwaggerSchema("Recibe un número, aquí se coloca el id de la unidad de medida del peso bruto.", Nullable = false)]
        [Required]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        [Required]
        public string CodigoUm { get; set; } = "KGM";

        [JsonPropertyName("ruc_empresa_currier")]
        [SwaggerSchema("Recibe un texto y es el ruc de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = false)]
        [MaxLength(11)]
        public string? RucEmpresaCurrier { get; set; }

        [JsonPropertyName("razon_social_currier")]
        [SwaggerSchema("Recibe un texto y es la razón social de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = false)]
        [MaxLength(200)]
        public string? RazonSocialCurrier { get; set; }

        [JsonPropertyName("registro_mtc_currier")]
        [SwaggerSchema("Recibe un texto y es el registro MTC de la empresa de Currier, este campo solo se llena si el tipo_transporte es 'PUBLICO'", Nullable = false)]
        [MaxLength(30)]
        public string? RegistroMtcCurrier { get; set; }

        [JsonPropertyName("indicador_vehiculo_conductor")]
        [SwaggerSchema("Recibe un booleano, este campo se marca como 'true' si 'tipo_transporte' es 'PUBLICO' y existen registros de vehículos y conductores.", Nullable = false)]
        public bool IndicadorVehiculoConductor { get; set; } = false;

        //[JsonPropertyName("conductor")]
        //public List<ConductorCreateDTO?> Conductor { get; set; }
        [JsonPropertyName("conductor")]
        [SwaggerSchema("Recibe la lista de enteros con los IDs de los conductores asociados a la guía de remisión.")]
        public List<int> ConductorId { get; set; }

        //[JsonPropertyName("unidad_transporte")]
        //public List<UnidadTransporteCreateDTO?> UnidadTransporte { get; set; }
        [JsonPropertyName("transporte")]
        [SwaggerSchema("Recibe la lista de enteros con los IDs de las unidades de transporte asociados a la guía de remisión.")]
        public List<int> TransporteId { get; set; }

        [JsonPropertyName("transportista_id")]
        [SwaggerSchema("Recibe un número, el id del transportista", Nullable = true)]
        public int? TransportistaId { get; set; }

        [JsonPropertyName("transportista")]
        [SwaggerSchema("Recibe un objeto de tipo transportista", Nullable = true)]
        public TransportistaDTO? Transportista { get; set; }
    }
}
