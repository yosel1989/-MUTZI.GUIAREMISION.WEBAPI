using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<(int GuiaId, string NumeroGuia)> CreateGuiaTransportistaTransactionalAsync(
            Establecimiento remitente,
            DOMAIN.Entities.Transportista remiteTransportista,
            Establecimiento destinatario,
            List<DOMAIN.Entities.ConductorGuid> conductor,
            List<DOMAIN.Entities.UnidadTransporteGuid> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.GuiaRemisionDatoEnvio datosEnvio,
            DOMAIN.Entities.GuiaRemisionOrigen origen,
            List<DOMAIN.Entities.GuiaRemisionDestino> destino,
            List<DOMAIN.Entities.GuiaRemisionDetalle> detalles,
            List<DOMAIN.Entities.DocRelacionado> docRelacionados,
            List<DOMAIN.Entities.EmailDestinatarioGuid> emailDestinatarioGuia,
            List<DOMAIN.Entities.EmailRemiteTransportistaGuid> emailRemiteTransportistas
            );
        //Remitente
        Task<DOMAIN.Entities.Transportista> GetRemiteTransportistaByRucAsync(string numeroDocumento);
        Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision);
    }
}
