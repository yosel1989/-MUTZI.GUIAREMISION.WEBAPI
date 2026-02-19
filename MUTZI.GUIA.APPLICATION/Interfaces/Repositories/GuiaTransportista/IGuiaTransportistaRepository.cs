namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<int> CreateGuiaTransportistaTransactionalAsync(
            DOMAIN.Entities.Remitente remitente,
            DOMAIN.Entities.RemiteTransportistum remiteTransportista,
            DOMAIN.Entities.Destinatario destinatario,
            List<DOMAIN.Entities.Conductor> conductor,
            List<DOMAIN.Entities.Transporte> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.Origen origen,
            List<DOMAIN.Entities.Destino> destino,
            List<DOMAIN.Entities.GuiaDetalle> detalles,
            List<DOMAIN.Entities.EmailDestinatarioGuid> emailDestinatarioGuia,
            List<DOMAIN.Entities.EmailRemiteTransportistaGuid> emailRemiteTransportistas
            );
        //Remitente
        Task<DOMAIN.Entities.RemiteTransportistum> GetRemiteTransportistaByRucAsync(string ruc);
    }
}
