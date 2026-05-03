using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista
{
    public class GuiaTransportistaUpdateDTO
    {
        [JsonPropertyName("fecha")]
        [SwaggerSchema("Recibe un texto de fecha en formato: yyyy-MM-dd; por ejemplo: 2025-09-23", Nullable = false, Title = "dddd")]
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

        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoUpdateDTO>? DocRelacionado { get; set; }

        [JsonPropertyName("empleado_id_edicion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDEdicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha editado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreEdicion { get; set; }

        // EstablecimientoRemitente
        //[JsonPropertyName("remitente")]
        //[Required]
        //public RemitenteCreateGuiaDTO EstablecimientoRemitente { get; set; }

        [JsonPropertyName("remite_transportista_id")]
        [Required]
        public int RemiteTransportistaId { get; set; }

        [JsonPropertyName("email_remite_transportista")]
        [SwaggerSchema("Recibe un texto, aquí se coloca hasta máximo 3 correos del remite_transportista", Nullable = true)]
        public List<EmailRemiteTransportistaGuiaUpdateDTO>? EmailRemiteTransportistaGuia { get; set; }

        [JsonPropertyName("destinatario")]
        [Required]
        public int DestinatarioId { get; set; }

        [JsonPropertyName("email_destinatario")]
        [SwaggerSchema("Recibe un texto, aquí se coloca hasta máximo 3 correos del destinatario", Nullable = true)]
        public List<EmailDestinatarioGuiaUpdateDTO>? EmailDestinatarioGuia { get; set; }

        [JsonPropertyName("datos_envio")]
        [Required]
        public DatosEnvioGTUpdateDTO DatosEnvio { get; set; }

        [JsonPropertyName("origen")]
        [Required]
        public OrigenUpdateDTO Origen { get; set; }

        [JsonPropertyName("destino")]
        [Required]
        public List<DestinoUpdateDTO> Destino { get; set; }

        [JsonPropertyName("productos")]
        [Required]
        public List<ProductosUpdateDTO> Productos { get; set; }
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
