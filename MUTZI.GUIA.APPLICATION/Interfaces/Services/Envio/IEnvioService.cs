namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio
{
    public interface IEnvioService
    {
        Task<string> EnviarGuiaElectronicaAsync(string ruc, string guiaJson);
        //Task<object> ProcesarGuiaRemisionAsync(string ruc, GuiaRemitenteCreateDTO guiaRemisionDto);
        //Task<object> ReenviarGuiaRemisionAsync(string ruc, int guiaId);
        //Task<object> ProcesarGuiaTransportistaAsync(string ruc, GuiaTransportistaCreateDTO guiaTransportistaDto);
        Task<object> ReenviarGuiaTransportistaAsync(string ruc, int guiaId);
        //void EscribirLog(string tipoGuia, string ruc, string numeroGuia, string estado, string ticket, string detalle);
    }
}
