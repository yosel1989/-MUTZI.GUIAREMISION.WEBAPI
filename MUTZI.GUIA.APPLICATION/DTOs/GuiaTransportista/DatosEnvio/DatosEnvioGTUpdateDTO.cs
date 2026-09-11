using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio
{
    [SwaggerSchema("Recibe un objeto datos_envio de la guía transportista, aquí se detala los detalles del envío para actualizar", Nullable = false)]
    public class DatosEnvioGTUpdateDTO
    {
        [JsonPropertyName("datos_envio_id")]
        [SwaggerSchema("Recibe un entero, aquí se coloca el ID de datos_envio que se va a actualizar", Nullable = false)]
        [Required]
        public int DatosEnvioId { get; set; }

        [JsonPropertyName("fecha_envio")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PRIVADO", Nullable = false)]
        [StringLength(10)]
        [MinLength(10)]
        public string? FechaEnvio { get; set; }

        [JsonPropertyName("fecha_entrega_transportista")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23, solo si es PUBLICO", Nullable = false)]
        [StringLength(10)]
        [MinLength(10)]
        public string? FechaEntregaTransportista { get; set; }

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

        // Conductor asociado
        [JsonPropertyName("conductor_guia")]
        [SwaggerSchema("Recibe una lista de conductores asociados a datos_envio dentro de la guía", Nullable = false)]
        [Required]
        public List<ConductorGuiaUpdateDTO> ConductorGuid { get; set; }

        // UnidadTransporte asociado
        [JsonPropertyName("transporte_guia")]
        [SwaggerSchema("Recibe una lista de transportes asociados a datos_envio dentro de la guía", Nullable = false)]
        [Required]
        public List<TransportUnitGuiaUpdateDTO> TransporteGuid { get; set; }
    }
}
