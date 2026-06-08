using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista
{
    public class GuiaTransportistaDTO
    {
        [JsonPropertyName("id")]
        [SwaggerSchema("Devuelve en entero el ID de la guía transportista.", Nullable = false)]
        public int GuiaID { get; set; }

        [JsonPropertyName("tipo_guia")]
        [SwaggerSchema("Devuelve en texto el tipo de guía: REMITENTE ó TRANSPORTISTA", Nullable = false)]
        public string TipoGuia { get; set; }

        [JsonPropertyName("ruc")]
        [SwaggerSchema("Devuelve en texto el ruc del facturador asociado", Nullable = false)]
        public string Ruc { get; set; }

        [JsonPropertyName("serie_correlativo")]
        public string SerieCorrelativo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; } // HH:mm:ss
        public string? Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoDTO>? DocRelacionados { get; set; }

        // Remitente
        public EstablecimientoDTO Remitente { get; set; }

        [JsonPropertyName("remite_transportista")]
        public TransportistaDTO RemiteTransportista { get; set; }

        public EstablecimientoDTO Destinatario { get; set; }

        public List<EmailDestinatarioGuiaDTO> DestinatarioEmail { get; set; } = new List<EmailDestinatarioGuiaDTO>();

        [JsonPropertyName("datos_envio")]
        public DatosEnvioGT_DTO DatosEnvio { get; set; }
        public OrigenDTO Origen { get; set; }
        public List<DestinoDTO> Destino { get; set; }
        public List<GuiaRemisionDetalleDTO> Productos { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }
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
