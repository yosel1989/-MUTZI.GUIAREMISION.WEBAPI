using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class GuiaRemitenteUpdateDTO
    {
        public string TipoTransporte { get; set; }
        public string ClaseTraslado { get; set; }
        public string SerieCorrelativo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Observacion { get; set; }
        // Documento de referencia (factura/boleta vinculada)
        public string TipoDocRef { get; set; }
        public string NumeroDocRef { get; set; }
        public string RucDocRef { get; set; }
        public DatosEnvioCreateDTO datosEnvioDto { get; set; }
        public DestinatarioCreateDTO datosDestinoDto { get; set; }
        public List<ProductosCreateDTO> productosDto { get; set; }
    }
}
