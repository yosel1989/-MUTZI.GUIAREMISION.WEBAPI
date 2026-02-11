using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio
{
    [SwaggerSchema("Recibe un objeto datos_envio de la guía transportista, aquí se detala los detalles del envío", Nullable=false)]
    public class DatosEnvioGTCreateDTO
    {
        //public string MotivoTraslado { get; set; }
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

        [JsonPropertyName("codigo_um")]
        [SwaggerSchema("Recibe un texto, aquí se coloca la unidad de medida del peso bruto. Ejm: KGM ó TNE", Nullable = false)]
        [Required]
        public string CodigoUM { get; set; } = "KGM";

        //EN caso de subcontratista
        [JsonPropertyName("ruc_empresa_currier")]
        [SwaggerSchema("Recibe un texto y es el ruc de la empresa de Currier, este campo solo se llena si el pagador del flete es un subcontratador", Nullable = false)]
        [MaxLength(11)]
        public string? RucEmpresaCurrier { get; set; }

        [JsonPropertyName("razon_social_currier")]
        [SwaggerSchema("Recibe un texto y es la razón social de la empresa de Currier, este campo solo se llena si el pagador del flete es un subcontratador", Nullable = false)]
        [MaxLength(200)]
        public string? RazonSocialCurrier { get; set; }

        [JsonPropertyName("registro_mtc_currier")]
        [SwaggerSchema("Recibe un texto y es el registro MTC de la empresa de Currier, este campo solo se llena si el pagador del flete es un subcontratador", Nullable = false)]
        [MaxLength(30)]
        public string? RegistroMtcCurrier { get; set; }

        // Conductor
        [JsonPropertyName("conductor")]
        [SwaggerSchema("Recibe un objeto conductor", Nullable = false)]
        [Required]
        public List<ConductorCreateDTO> Conductor { get; set; }

        // Unidad de transporte
        [JsonPropertyName("unidad_transporte")]
        [SwaggerSchema("Recibe un objeto unidad_transporte", Nullable = false)]
        [Required]
        public List<UnidadTransporteCreateDTO> UnidadTransporte { get; set; }
    }
}
