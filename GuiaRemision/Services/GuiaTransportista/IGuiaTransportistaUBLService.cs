using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaTransportista;

namespace GuiaRemision.Services.GuiaTransportista
{
    public interface IGuiaTransportistaUBLService
    {
        Task <string> GenerarGuiaTransportistaJson(GuiaTransportistaDTO dto);
        public string RutaDocPendiente(GuiaTransportistaDTO guia, string ruc);
        public string getNombreDocumentoFile(GuiaTransportistaDTO guia, string ruc);
    }
}
