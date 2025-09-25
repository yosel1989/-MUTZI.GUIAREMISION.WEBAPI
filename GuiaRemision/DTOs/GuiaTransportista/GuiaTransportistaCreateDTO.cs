using GuiaRemision.Constantes;
using GuiaRemision.DTOs.GuiaRemitente.DatosDestino;
using GuiaRemision.DTOs.GuiaRemitente.DatosEnvio;
using GuiaRemision.DTOs.GuiaRemitente.Destino;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.Productos;
using GuiaRemision.DTOs.GuiaRemitente.Remitente;
using GuiaRemision.DTOs.GuiaTransportista.DatosEnvio;
using GuiaRemision.DTOs.GuiaTransportista.RemiteTransportista;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuiaRemision.DTOs.GuiaTransportista
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
        [SwaggerSchema("Recibe un texto, donde se detalla una observación o comentario sobre la guía de remisión transportista", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        [JsonPropertyName("tipo_doc_ref")]
        [SwaggerSchema("Recibe un texto, es el tipo de documento vinculado como referencia en la guía de remisión transportista, los tipos definidos son: FACTURA, BOLETA, NOTA DE CREDITO Y NOTA DE DEBITO", Nullable = false)]
        [MaxLength(20)]
        [Required]
        public string TipoDocRef { get; set; }

        [JsonPropertyName("numero_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se hace referencia al numero del documento vinculado en la guía de remisión transportista. Ejm: F025-00000003", Nullable = false)]
        [MaxLength(21)]
        [Required]
        public string NumeroDocRef { get; set; }

        [JsonPropertyName("ruc_doc_ref")]
        [SwaggerSchema("Recibe un texto, aquí se coloca el ruc del documento vinculado como referencia en la guía de remisión transportista.", Nullable = false)]
        [StringLength(11)]
        [MinLength(11)]
        [Required]
        public string RucDocRef { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        [SwaggerSchema("Recibe un entero, aquí se soloca el ID del empleado que ha realizado la transacción", Nullable = false)]
        [Required]
        public int EmpleadoIDCreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        [SwaggerSchema("Recibe un texto, se coloca el nombre del empleado que ha registrado la transacción en el sistema", Nullable = false)]
        [MaxLength(100)]
        [Required]
        public string EmpleadoNombreCreacion { get; set; }

        // Remitente
        [JsonPropertyName("remitente")]
        [SwaggerSchema("Recibe un objeto remitente, si es nuevo remitente la api crea un nuevo remitente en la bbdd")]
        [Required]
        public RemitenteCreateDTO Remitente { get; set; }

        [JsonPropertyName("remite_transportista")]
        [Required]
        public RemiteTransportistaCreateDTO RemiteTransportista { get; set; }

        [JsonPropertyName("destinatario")]
        [Required]
        public DestinatarioCreateDTO Destinatario { get; set; }

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
        public string DocumentoAdicinalTipo()
        {
            if (string.IsNullOrEmpty(this.TipoDocRef))
                throw new System.Exception("tipo_doc_ref no puede estar vacío al generar el tipo de documento adicional.");

            switch (this.TipoDocRef.ToUpper())
            {
                case "FACTURA":
                    return "01"; // Factura
                case "BOLETA":
                    return "03"; // Boleta
                case "NOTA DE CREDITO":
                    return "07"; // Nota de crédito
                case "NOTA DE DEBITO":
                    return "08"; // Nota de débito
                default:
                    throw new System.Exception("Catálogo No. 01: (tipo_doc_ref)Tipo de documento vinculado como referencia no definido.");
            }
        }
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
