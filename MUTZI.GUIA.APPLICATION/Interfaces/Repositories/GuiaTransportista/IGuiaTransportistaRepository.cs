namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<int> CreateGuiaTransportistaTransactionalAsync(
            DOMAIN.Entities.Remitente remitente,
            DOMAIN.Entities.RemiteTransportista remiteTransportista,
            DOMAIN.Entities.Destinatario destinatario,
            DOMAIN.Entities.Conductor conductor,
            DOMAIN.Entities.Transporte transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.DatosEnvio datosEnvio,
            DOMAIN.Entities.Origen origen,
            List<DOMAIN.Entities.Destino> destino,
            List<DOMAIN.Entities.GuiaDetalle> detalles
            );
        //Remitente
        Task<DOMAIN.Entities.RemiteTransportista> GetRemiteTransportistaByRucAsync(string ruc);
    }
}
