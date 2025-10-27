using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista
{
    public interface IGuiaTransportistaUBLService
    {
        Task <string> GenerarGuiaTransportistaJson(GuiaTransportistaDTO dto);
        public string RutaDocPendiente(GuiaTransportistaDTO guia, string ruc);
        public string getNombreDocumentoFile(GuiaTransportistaDTO guia, string ruc);
    }
}
