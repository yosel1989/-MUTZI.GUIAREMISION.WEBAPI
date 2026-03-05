using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista
{
    public class GuiaTransportistaDTO
    {
        public int GuiaID { get; set; }
        public string SerieCorrelativo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; } // HH:mm:ss
        public string? Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        [JsonPropertyName("doc_relacionado")]
        public List<DocRelacionadoDTO>? DocRelacionados { get; set; }

        // Remitente
        public RemitenteDTO Remitente { get; set; }
        public RemiteTransportistaDTO RemiteTransportista { get; set; }
        public DestinatarioDTO Destinatario { get; set; }
        public DatosEnvioGT_DTO DatosEnvio { get; set; }
        public OrigenDTO Origen { get; set; }
        public List<DestinoDTO> Destino { get; set; }
        public List<ProductosDTO> Productos { get; set; }
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
