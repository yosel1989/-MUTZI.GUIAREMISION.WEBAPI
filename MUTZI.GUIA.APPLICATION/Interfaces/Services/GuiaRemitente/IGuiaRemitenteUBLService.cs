using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente
{
    public interface IGuiaRemitenteUBLService
    {
        Task<string> GenerarGuiaRemisionJson(GuiaRemitenteDTO guia);
        public string RutaDocPendiente(GuiaRemitenteDTO guia, string ruc);
        public string getNombreDocumentoFile(GuiaRemitenteDTO guia, string ruc);
    }
}
