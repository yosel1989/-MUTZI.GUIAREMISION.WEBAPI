namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<(int GuiaId, string NumeroGuia)> CreateGuiaTransportistaTransactionalAsync(
            DOMAIN.Entities.Remitente remitente,
            DOMAIN.Entities.RemiteTransportistum remiteTransportista,
            DOMAIN.Entities.Destinatario destinatario,
            List<DOMAIN.Entities.ConductorGuid> conductor,
            List<DOMAIN.Entities.TransporteGuid> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.Origen origen,
            List<DOMAIN.Entities.Destino> destino,
            List<DOMAIN.Entities.GuiaDetalle> detalles,
            List<DOMAIN.Entities.DocRelacionado> docRelacionados,
            List<DOMAIN.Entities.EmailDestinatarioGuid> emailDestinatarioGuia,
            List<DOMAIN.Entities.EmailRemiteTransportistaGuid> emailRemiteTransportistas
            );
        //Remitente
        Task<DOMAIN.Entities.RemiteTransportistum> GetRemiteTransportistaByRucAsync(string numeroDocumento);
        Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision);
    }
}
