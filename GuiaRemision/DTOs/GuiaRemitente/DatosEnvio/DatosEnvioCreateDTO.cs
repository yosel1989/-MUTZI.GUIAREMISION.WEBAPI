using GuiaRemision.DTOs.GuiaRemitente.Conductor;
using GuiaRemision.DTOs.GuiaRemitente.Destino;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaRemitente.DatosEnvio
{
    [SwaggerSchema("Recibe un objeto datos_envio, aquí se detala los detalles del envío", Nullable = false)]
    public class DatosEnvioCreateDTO
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
        [SwaggerSchema("Recibe un texto, aquí debe colocarse el peso bruto de todo el envío, Ejm: 20, 50, etc.", Nullable = false)]
        [Required]
        public string PesoBruto { get; set; }

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        [Required]
        public string CodigoUM { get; set; } = "KGM";

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

        [JsonPropertyName("conductor")]
        public ConductorCreateDTO? Conductor { get; set; }

        [JsonPropertyName("unidad_transporte")]
        public UnidadTransporteCreateDTO? UnidadTransporte { get; set; }
    }
}
