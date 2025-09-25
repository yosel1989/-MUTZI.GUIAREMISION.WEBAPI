using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaTransportista;

namespace GuiaRemision.Services.Envio
{
    public interface IEnvioService
    {
        Task<string> EnviarGuiaElectronicaAsync(string ruc, string guiaJson);
        Task<object> ProcesarGuiaRemisionAsync(string ruc, GuiaRemitenteCreateDTO guiaRemisionDto);
        Task<object> ProcesarGuiaTransportistaAsync(string ruc, GuiaTransportistaCreateDTO guiaTransportistaDto);
        void EscribirLog(string tipoGuia, string ruc, string numeroGuia, string estado, string ticket, string detalle);
    }
}
