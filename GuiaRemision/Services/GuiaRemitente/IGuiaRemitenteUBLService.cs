using GuiaRemision.DTOs.GuiaRemitente;

namespace GuiaRemision.Services.GuiaRemision
{
    public interface IGuiaRemitenteUBLService
    {
        Task<string> GenerarGuiaRemisionJson(GuiaRemitenteDTO guia);
        public string RutaDocPendiente(GuiaRemitenteDTO guia, string ruc);
        public string getNombreDocumentoFile(GuiaRemitenteDTO guia, string ruc);
    }
}
