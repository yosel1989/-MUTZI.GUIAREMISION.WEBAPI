using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista
{
    public class GuiaTransportistaCreateDTO
    {

        [JsonPropertyName("fecha")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false, Title ="dddd")]
        [StringLength(10)]
        [MinLength(10)]
        [Required]
        public string Fecha { get; set; } // yyyy-MM-dd

        [JsonPropertyName("hora")]
        [SwaggerSchema("Recibe un texto de hora en formato de 24 horas(hh:mm:ss); por ejemplo: 14:35:00", Nullable = false)]
        [StringLength(8)]
        [MinLength(8)]
        [Required]
        public string Hora { get; set; } // HH:mm:ss

        [JsonPropertyName("observacion")]
        [SwaggerSchema("Recibe un texto, donde se detalla una observación o comentario sobre la guía de remisión transportista", Nullable = true)]
        [MaxLength(100)]
        public string? Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoCreateDTO>? DocRelacionado { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreCreacion { get; set; }

        // EstablecimientoRemitente
        [JsonPropertyName("remitente")]
        [Required]
        public RemitenteCreateGuiaDTO Remitente { get; set; }

        [JsonPropertyName("remite_transportista")]
        [Required]
        public RemiteTransportistaCreateGuiaDTO RemiteTransportista { get; set; }

        [JsonPropertyName("destinatario")]
        [Required]
        public DestinatarioCreateGuiaDTO Destinatario { get; set; }

        [JsonPropertyName("datos_envio")]
        [Required]
        public DatosEnvioGTCreateDTO DatosEnvio { get; set; }

        [JsonPropertyName("origen")]
        [Required]
        public OrigenCreateDTO Origen { get; set; }

        [JsonPropertyName("destino")]
        [Required]
        public List<DestinoCreateDTO> Destino { get; set; }

        [JsonPropertyName("productos")]
        [Required]
        public List<ProductosCreateDTO> Productos { get; set; }
        public string getTipoCodigoDocumento(string tipo)
        {
            if (tipo == "DNI")
                return EspacioNombreGuiaRemision._DocumentType01;
            if (tipo == "CE")
                return EspacioNombreGuiaRemision._DocumentType04;
            if (tipo == "RUC")
                return EspacioNombreGuiaRemision._DocumentType06;
            if (tipo == "PASAPORTE")
                return EspacioNombreGuiaRemision._DocumentType07;
            throw new System.Exception("Tipo de documento de identidad no especificado, coloque entre: DNI, CE, RUC, PASAPORTE");
        }
    }
}
