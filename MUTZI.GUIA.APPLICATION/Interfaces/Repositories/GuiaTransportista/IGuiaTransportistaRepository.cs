using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista
{
    public interface IGuiaTransportistaRepository
    {
        Task<DOMAIN.Entities.GuiaRemision> GetByIdAsync(int id);
        Task<(int GuiaId, string NumeroGuia)> CreateGuiaTransportistaTransactionalAsync(
            EntityBranch remitente,
            EntityBranchSerie remitenteSerie,
            Entity remiteTransportista,
            EntityBranch destinatario,
            List<DOMAIN.Entities.GuiaRemisionConductor> conductor,
            List<DOMAIN.Entities.GuiaRemisionUnidadTransporte> transporte,
            DOMAIN.Entities.GuiaRemision guiaRemision,
            DOMAIN.Entities.GuiaRemisionDatoEnvio datosEnvio,
            DOMAIN.Entities.GuiaRemisionOrigen origen,
            List<DOMAIN.Entities.GuiaRemisionDestino> destino,
            List<DOMAIN.Entities.GuiaRemisionDetalle> detalles,
            List<DOMAIN.Entities.GuiaRemisionDocumentoRelacionado> docRelacionados,
            List<DOMAIN.Entities.GuiaRemisionEmailDestinatario> emailDestinatarioGuia,
            List<DOMAIN.Entities.EmailRemiteTransportistaGuid> emailRemiteTransportistas
            );
        //Remitente
        Task<DOMAIN.Entities.Transportista> GetRemiteTransportistaByRucAsync(string numeroDocumento);
        Task UpdateAsync(DOMAIN.Entities.GuiaRemision guiaRemision);
    }
}
