using GuiaRemision.Constantes;
using GuiaRemision.DTOs.GuiaRemitente.Destinatario;
using GuiaRemision.DTOs.GuiaRemitente.Destino;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.Productos;
using GuiaRemision.DTOs.GuiaRemitente.Remitente;
using GuiaRemision.DTOs.GuiaTransportista.DatosEnvio;
using GuiaRemision.DTOs.GuiaTransportista.RemiteTransportista;
using System.ComponentModel.DataAnnotations;

namespace GuiaRemision.DTOs.GuiaTransportista
{
    public class GuiaTransportistaDTO
    {
        public int GuiaID { get; set; }
        public string SerieCorrelativo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; } // HH:mm:ss
        public string Observacion { get; set; }

        // Documento de referencia (factura/boleta vinculada)
        public string TipoDocRef { get; set; }
        public string NumeroDocRef { get; set; }
        public string RucDocRef { get; set; }

        // Remitente
        public RemitenteDTO Remitente { get; set; }
        public RemiteTransportistaDTO RemiteTransportista { get; set; }
        public DestinatarioDTO Destinatario { get; set; }
        public DatosEnvioGT_DTO DatosEnvio { get; set; }
        public OrigenDTO Origen { get; set; }
        public List<DestinoDTO> Destino { get; set; }
        public List<ProductosDTO> Productos { get; set; }
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
